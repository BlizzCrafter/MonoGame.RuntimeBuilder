using Microsoft.Xna.Framework.Content.Pipeline;

namespace MonoGame.RuntimeBuilder.MGCB
{
#pragma warning disable 1591
    public class PipelineImporterContext : ContentImporterContext
    {
        private readonly PipelineManager _manager;

        public PipelineImporterContext(PipelineManager manager)
        {
            _manager = manager;
        }

        public override string IntermediateDirectory { get { return _manager.IntermediateDirectory; } }
        public override string OutputDirectory { get { return _manager.OutputDirectory; } }
        public override ContentBuildLogger Logger { get { return _manager.Logger; } }

        public override void AddDependency(string filename)
        {
        }
    }
}
