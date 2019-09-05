namespace MonoGame.Forms.DX.RuntimeBuilder
{
    /// <summary>
    /// Shows how many content files were build successfully.
    /// </summary>
    public struct BuildResult
    {
        /// <summary>
        /// Successful builds
        /// </summary>
        public int Success;

        /// <summary>
        /// Builds with errors
        /// </summary>
        public int Error;
    }
}
