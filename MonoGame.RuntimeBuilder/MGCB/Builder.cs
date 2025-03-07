﻿using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Content.Pipeline.Builder;

namespace MonoGame.RuntimeBuilder.MGCB
{
#pragma warning disable 1591
    public class BuildContent
    {
        public bool LaunchDebugger = false;
        public bool Quiet = false;
        public List<string> ResponseFiles
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }

        public void SetWorkingDir(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        private string _outputDir = string.Empty;
        public void SetOutputDir(string path)
        {
            _outputDir = Path.GetFullPath(path);
        }

        private string _intermediateDir = string.Empty;
        public void SetIntermediateDir(string path)
        {
            _intermediateDir = Path.GetFullPath(path);
        }

        public bool Rebuild = false;

        public bool Clean = false;

        public bool Incremental = false;

        public readonly List<string> References = new List<string>();

        public TargetPlatform Platform = TargetPlatform.Windows;

        public GraphicsProfile Profile = GraphicsProfile.HiDef;

        public string Config = string.Empty;

        public string Importer = null;

        public ContentBuildLogger Logger = new ConsoleLogger();

        public void SetProcessor(string processor)
        {
            _processor = processor;

            // If you are changing the processor then reset all 
            // the processor parameters.
            _processorParams.Clear();
        }

        private string _processor = null;
        private readonly OpaqueDataDictionary _processorParams = new OpaqueDataDictionary();

        public void AddProcessorParam(string nameAndValue)
        {
            var keyAndValue = nameAndValue.Split('=', ':');
            if (keyAndValue.Length != 2)
            {
                // Do we error out or something?
                return;
            }

            _processorParams.Remove(keyAndValue[0]);
            _processorParams.Add(keyAndValue[0], keyAndValue[1]);
        }
        
        public void OnBuild(string sourceFile)
        {
            string link = null;
            if (sourceFile.Contains(";"))
            {
                var split = sourceFile.Split(';');
                sourceFile = split[0];

                if (split.Length > 0)
                    link = split[1];
            }

            // Make sure the source file is absolute.
            if (!Path.IsPathRooted(sourceFile))
                sourceFile = Path.Combine(Directory.GetCurrentDirectory(), sourceFile);

            // link should remain relative, absolute path will get set later when the build occurs

            sourceFile = PathHelper.Normalize(sourceFile);

            // Remove duplicates... keep this new one.
            var previous = _content.FindIndex(e => string.Equals(e.SourceFile, sourceFile, StringComparison.InvariantCultureIgnoreCase));
            if (previous != -1)
                _content.RemoveAt(previous);

            // Create the item for processing later.
            var item = new ContentItem
            {
                SourceFile = sourceFile,
                OutputFile = link,
                Importer = Importer,
                Processor = _processor,
                ProcessorParams = new OpaqueDataDictionary()
            };
            _content.Add(item);

            // Copy the current processor parameters blind as we
            // will validate and remove invalid parameters during
            // the build process later.
            foreach (var pair in _processorParams)
                item.ProcessorParams.Add(pair.Key, pair.Value);
        }

        public void OnCopy(string sourceFile)
        {
            string link = null;
            if (sourceFile.Contains(";"))
            {
                var split = sourceFile.Split(';');
                sourceFile = split[0];

                if (split.Length > 0)
                    link = split[1];
            }

            if (!Path.IsPathRooted(sourceFile))
                sourceFile = Path.Combine(Directory.GetCurrentDirectory(), sourceFile);

            sourceFile = PathHelper.Normalize(sourceFile);

            // Remove duplicates... keep this new one.
            var previous = _copyItems.FindIndex(e => string.Equals(e.SourceFile, sourceFile, StringComparison.InvariantCultureIgnoreCase));
            if (previous != -1)
                _copyItems.RemoveAt(previous);

            _copyItems.Add(new CopyItem { SourceFile = sourceFile, Link = link });
        }

        public bool CompressContent = false;

        public class ContentItem
        {
            public string SourceFile;

            // This refers to the "Link" which can override the default output location
            public string OutputFile;
            public string Importer;
            public string Processor;
            public OpaqueDataDictionary ProcessorParams;
        }

        public class CopyItem
        {
            public string SourceFile;
            public string Link;
        }

        private readonly List<ContentItem> _content = new List<ContentItem>();

        private readonly List<CopyItem> _copyItems = new List<CopyItem>();

        private PipelineManager _manager;

        public bool HasWork
        {
            get { return _content.Count > 0 || _copyItems.Count > 0 || Clean; }
        }

        string ReplaceSymbols(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                return parameter;
            return parameter
                .Replace("$(Platform)", Platform.ToString())
                .Replace("$(Configuration)", Config)
                .Replace("$(Config)", Config)
                .Replace("$(Profile)", this.Profile.ToString());
        }

        public async Task<BuildResult> Build()
        {
            int successCount, errorCount;

            var projectDirectory = PathHelper.Normalize(Directory.GetCurrentDirectory());

            var outputPath = ReplaceSymbols(_outputDir);
            if (!Path.IsPathRooted(outputPath))
                outputPath = PathHelper.Normalize(Path.GetFullPath(Path.Combine(projectDirectory, outputPath)));

            var intermediatePath = ReplaceSymbols(_intermediateDir);
            if (!Path.IsPathRooted(intermediatePath))
                intermediatePath = PathHelper.Normalize(Path.GetFullPath(Path.Combine(projectDirectory, intermediatePath)));

            _manager = new PipelineManager(projectDirectory, outputPath, intermediatePath)
            {
                Logger = Logger,
                CompressContent = CompressContent
            };

            // If the intent is to debug build, break at the original location
            // of any exception, eg, within the actual importer/processor.
            if (LaunchDebugger)
                _manager.RethrowExceptions = false;

            // Feed all the assembly references to the pipeline manager
            // so it can resolve importers, processors, writers, and types.
            foreach (var r in References)
            {
                var assembly = r;
                if (!Path.IsPathRooted(assembly))
                    assembly = Path.GetFullPath(Path.Combine(projectDirectory, assembly));
                _manager.AddAssembly(assembly);
            }

            // Load the previously serialized list of built content.
            var contentFile = Path.Combine(intermediatePath, PipelineBuildEvent.Extension);
            var previousContent = SourceFileCollection.Read(contentFile);

            // If the target changed in any way then we need to force
            // a full rebuild even under incremental builds.
            var targetChanged = previousContent.Config != Config ||
                                previousContent.Platform != Platform ||
                                previousContent.Profile != Profile;

            // First clean previously built content.
            for (int i = 0; i < previousContent.SourceFiles.Count; i++)
            {
                var sourceFile = previousContent.SourceFiles[i];

                // This may be an old file (prior to MG 3.7) which doesn't have destination files:
                string destFile = null;
                if (i < previousContent.DestFiles.Count)
                {
                    destFile = previousContent.DestFiles[i];
                }

                var inContent = _content.Any(e => string.Equals(e.SourceFile, sourceFile, StringComparison.InvariantCultureIgnoreCase));
                var cleanOldContent = !inContent && !Incremental;
                var cleanRebuiltContent = inContent && (Rebuild || Clean);
                if (cleanRebuiltContent || cleanOldContent || targetChanged)
                    _manager.CleanContent(sourceFile, destFile);
            }

            // TODO: Should we be cleaning copy items?  I think maybe we should.

            var newContent = new SourceFileCollection
            {
                Profile = _manager.Profile = Profile,
                Platform = _manager.Platform = Platform,
                Config = _manager.Config = Config
            };
            errorCount = 0;
            successCount = 0;

            // Before building the content, register all files to be built. (Necessary to
            // correctly resolve external references.)
            foreach (var c in _content)
            {
                try
                {
                    _manager.RegisterContent(c.SourceFile, c.OutputFile, c.Importer, c.Processor, c.ProcessorParams);
                }
                catch
                {
                    // Ignore exception. Exception will be handled below.
                }
            }

            foreach (var c in _content)
            {
                try
                {
                    await _manager.BuildContent(c.SourceFile,
                                          c.OutputFile,
                                          c.Importer,
                                          c.Processor,
                                          c.ProcessorParams);

                    newContent.SourceFiles.Add(c.SourceFile);
                    newContent.DestFiles.Add(c.OutputFile);

                    ++successCount;
                }
                catch (InvalidContentException ex)
                {
                    var message = string.Empty;
                    if (ex.ContentIdentity != null && !string.IsNullOrEmpty(ex.ContentIdentity.SourceFilename))
                    {
                        message = ex.ContentIdentity.SourceFilename;
                        if (!string.IsNullOrEmpty(ex.ContentIdentity.FragmentIdentifier))
                            message += "(" + ex.ContentIdentity.FragmentIdentifier + ")";
                        message += ": ";
                    }
                    message += ex.Message;
                    Console.WriteLine(message);
                    ++errorCount;
                }
                catch (PipelineException ex)
                {
                    Console.Error.WriteLine("{0}: error: {1}", c.SourceFile, ex.Message);
                    if (ex.InnerException != null)
                        Console.Error.WriteLine(ex.InnerException.ToString());
                    ++errorCount;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("{0}: error: {1}", c.SourceFile, ex.Message);
                    if (ex.InnerException != null)
                        Console.Error.WriteLine(ex.InnerException.ToString());
                    ++errorCount;
                }
            }

            // If this is an incremental build we merge the list
            // of previous content with the new list.
            if (Incremental && !targetChanged)
            {
                newContent.Merge(previousContent);
                _manager.ContentStats.MergePreviousStats();
            }

            // Delete the old file and write the new content 
            // list if we have any to serialize.
            FileHelper.DeleteIfExists(contentFile);
            if (newContent.SourceFiles.Count > 0)
                newContent.Write(contentFile);

            // Process copy items (files that bypass the content pipeline)
            foreach (var c in _copyItems)
            {
                try
                {
                    // Figure out an asset name relative to the project directory,
                    // retaining the file extension.
                    // Note that replacing a sub-path like this requires consistent
                    // directory separator characters.
                    var relativeName = c.Link;
                    if (string.IsNullOrWhiteSpace(relativeName))
                        relativeName = c.SourceFile.Replace(projectDirectory, string.Empty)
                                            .TrimStart(Path.DirectorySeparatorChar)
                                            .TrimStart(Path.AltDirectorySeparatorChar);
                    var dest = Path.Combine(outputPath, relativeName);

                    // Only copy if the source file is newer than the destination.
                    // We may want to provide an option for overriding this, but for
                    // nearly all cases this is the desired behavior.
                    if (File.Exists(dest) && !Rebuild)
                    {
                        var srcTime = File.GetLastWriteTimeUtc(c.SourceFile);
                        var dstTime = File.GetLastWriteTimeUtc(dest);
                        if (srcTime <= dstTime)
                        {
                            if (string.IsNullOrEmpty(c.Link))
                                Console.WriteLine("Skipping {0}", c.SourceFile);
                            else
                                Console.WriteLine("Skipping {0} => {1}", c.SourceFile, c.Link);

                            // Copy the stats from the previous stats collection.
                            _manager.ContentStats.CopyPreviousStats(c.SourceFile);
                            continue;
                        }
                    }

                    var startTime = DateTime.UtcNow;

                    // Create the destination directory if it doesn't already exist.
                    var destPath = Path.GetDirectoryName(dest);
                    if (!Directory.Exists(destPath))
                        Directory.CreateDirectory(destPath);

                    File.Copy(c.SourceFile, dest, true);

                    // Destination file should not be read-only even if original was.
                    var fileAttr = File.GetAttributes(dest);
                    fileAttr = fileAttr & (~FileAttributes.ReadOnly);
                    File.SetAttributes(dest, fileAttr);

                    var buildTime = DateTime.UtcNow - startTime;

                    if (string.IsNullOrEmpty(c.Link))
                        Console.WriteLine("{0}", c.SourceFile);
                    else
                        Console.WriteLine("{0} => {1}", c.SourceFile, c.Link);

                    // Record content stats on the copy.
                    _manager.ContentStats.RecordStats(c.SourceFile, dest, "CopyItem", typeof(File), (float)buildTime.TotalSeconds);

                    ++successCount;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("{0}: error: {1}", c, ex.Message);
                    if (ex.InnerException != null)
                        Console.Error.WriteLine(ex.InnerException.ToString());

                    ++errorCount;
                }
            }

            // Dump the content build stats.
            _manager.ContentStats.Write(intermediatePath);

            return new BuildResult() { Success = successCount, Error = errorCount };
        }
    }
}
