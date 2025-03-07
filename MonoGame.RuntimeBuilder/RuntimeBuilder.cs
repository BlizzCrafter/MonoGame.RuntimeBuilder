using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.RuntimeBuilder.MGCB;
using System.Reflection;

namespace MonoGame.RuntimeBuilder
{
    /// <summary>
    /// This class is a wrapper around the original MonoGameContentBuilder (MGCB).
    /// It contains the same features as the MGCB but it has the additional ability to build content asynchronously.
    /// </summary>
    public class RuntimeBuilder
    {
        /// <summary>
        /// Set your <see cref="ContentBuildLogger"/> to receive content builder status reports.
        /// </summary>
        public ContentBuildLogger Logger
        {
            get { InitializationCheck(); return _BuildContent.Logger; }
            set { InitializationCheck(); _BuildContent.Logger = value; }
        }

        /// <summary>
        /// Clears the log of a <see cref="StringBuilderLogger"/>.
        /// </summary>
        public void ClearLog()
        {
            if (Logger is StringBuilderLogger) ((StringBuilderLogger)Logger).Clear();
        }

        /// <summary>
        /// An optional parameter which forces a full rebuild of all content.
        /// </summary>
        public bool Rebuild { set { InitializationCheck(); _BuildContent.Rebuild = value; } }

        /// <summary>
        /// Skip cleaning files not included in the current build. Useful for custom tools which only require a subset of the game content built.
        /// </summary>
        public bool Incremental { set { InitializationCheck(); _BuildContent.Incremental = value; } }

        /// <summary>
        /// Delete all previously built content and intermediate files. Only the /intermediateDir and /outputDir need to be defined for clean to do its job.
        /// </summary>
        public bool Clean { set { InitializationCheck(); _BuildContent.Clean = value; } }

        /// <summary>
        /// Mute the status reports.
        /// </summary>
        public bool Quiet { set { InitializationCheck(); _BuildContent.Quiet = value; } }

        /// <summary>
        /// The optional build configuration name from the build system. This is sometimes used as a hint in content processors.
        /// </summary>
        public string Config { set { InitializationCheck(); _BuildContent.Config = value; } }

        /// <summary>
        /// An optional parameter which defines the class name of the content importer for reading source content. If the option is omitted or used without a class name the default content importer for the source type is used.
        /// </summary>
        public string Importer { set { InitializationCheck(); _BuildContent.Importer = value; } }

        /// <summary>
        /// An optional parameter which defines the class name of the content processor for processing imported content. If the option is omitted used without a class name the default content processor for the imported content is used.
        /// <remarks>
        /// Note that when you change the processor all previously defined /processorParam are cleared.
        /// </remarks>
        /// </summary>
        public string Processor { set { InitializationCheck(); _BuildContent.SetProcessor(value); } }

        /// <summary>
        /// An optional parameter which adds one or more assembly references which contains importers, processors, or writers needed during content building.
        /// </summary>
        /// <param name="references"></param>
        public void AddReferences(params string[] references)
        {
            InitializationCheck();

            _BuildContent.References.AddRange(references);
        }

        /// <summary>
        /// Clear all but the default assembly references.
        /// </summary>
        public void ClearAllReferences()
        {
            InitializationCheck();

            _BuildContent.References.Clear();
            _BuildContent.References.Add(_ContentPipelineReference.Location);
        }

        private BuildContent _BuildContent;
        private Assembly _ContentPipelineReference;

        private void FinalizeBuild(int success, int error) => Logger.LogMessage($"\nBuild {success} succeeded, {error} failed.\n");

        private void InitializationCheck()
        {
            if (_BuildContent == null) throw new Exception("Please initialize the RuntimeBuilder first!");
        }

        /// <summary>
        /// Inititalize the <see cref="RuntimeBuilder"/> so you can use its capabilities to build content during runtime.
        /// </summary>
        /// <param name="workingDir">The working dir is where your raw content files are (automatically) beeing copied to before building them.</param>
        /// <param name="intermediateDir">It specifies the directory where all intermediate files are written. If this option is omitted the intermediate data will be put into the current working directory.</param>
        /// <param name="outputPath">It specifies the directory where all content is written. If this option is omitted the output will be put into the current working directory.</param>
        /// <param name="platform">Set the target platform for this build. It must be a member of the <see cref="TargetPlatform"/> enum.</param>
        /// <param name="profile">Set the target graphics profile for this build. It must be a member of the <see cref="GraphicsProfile"/> enum.</param>
        /// <param name="compress">Uses LZ4 compression to compress the contents of the XNB files. Content build times will increase with this option enabled. Compression is not recommended for platforms such as Android, Windows Phone 8 and Windows 8 as the app package is already compressed. This is not compatible with LZX compression used in XNA content.</param>
        public RuntimeBuilder(
            string workingDir, string intermediateDir, string outputPath,
            TargetPlatform platform,
            GraphicsProfile profile,
            bool compress)
        {
            Directory.CreateDirectory(workingDir);
            Directory.CreateDirectory(intermediateDir);
            Directory.CreateDirectory(outputPath);

            _BuildContent = new BuildContent();
            _BuildContent.SetWorkingDir(workingDir);
            _BuildContent.SetIntermediateDir(intermediateDir);
            _BuildContent.SetOutputDir(outputPath);

            _BuildContent.Platform = platform;
            _BuildContent.Profile = profile;
            _BuildContent.CompressContent = compress;

            _ContentPipelineReference = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(x => x.FullName.Contains("MonoGame.Framework.Content.Pipeline"));
            if (_ContentPipelineReference != null) _BuildContent.References.Add(_ContentPipelineReference.Location);
            else throw new DllNotFoundException("Couldn't find the MonoGame.Framework.Content.Pipeline.dll! Please make sure that it is available in the current App Domain.");
        }

        /// <summary>
        /// Builds the previously set raw content files asynchronously and outputs them as .XNB files.
        /// </summary>
        /// <returns></returns>
        public async Task BuildContent()
        {
            var result = await _BuildContent.Build();

            FinalizeBuild(result.Success, result.Error);
        }

        /// <summary>
        /// Builds raw content files specified in the files parameter and outputs them as .XNB files.
        /// </summary>
        /// <param name="files">The files you want to build. Please use absolute pathes.</param>
        /// <returns></returns>
        public async Task BuildContent(params string[] files)
        {
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                File.Copy(file, destinationFile, true);

                _BuildContent.OnBuild(fileName);
            }

            var result = await _BuildContent.Build();

            FinalizeBuild(result.Success, result.Error);
        }
    }
}
