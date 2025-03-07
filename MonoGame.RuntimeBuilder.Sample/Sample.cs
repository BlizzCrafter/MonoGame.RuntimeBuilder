using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.RuntimeBuilder.Sample
{
    public partial class Sample : Form
    {
        private RuntimeBuilder _RuntimeBuilder { get; set; }

        public Sample()
        {
            InitializeComponent();

            _RuntimeBuilder = new RuntimeBuilder(
                Path.Combine(Application.StartupPath, "working"),           // working directory
                Path.Combine(Application.StartupPath, "working", "obj"),    // intermediate directory
                Path.Combine(Application.StartupPath, "Content"),           // output directory
                TargetPlatform.Windows,                                     // target platform
                GraphicsProfile.Reach,                                      // graphics profile
                true)                                                       // compress the content
            {
                Logger = new StringBuilderLogger()                          // logger
            };

            // When the StringBuilderLogger logs a message, we write it to a RichTextBox and scroll to the end of it.
            ((StringBuilderLogger)_RuntimeBuilder.Logger).OnMessageLogged += (log) =>
            {
                richTextBoxLog.Text = log;
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            };
        }

        // Pick Files & Build Content
        private async void ButtonPickFiles_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                await _RuntimeBuilder.BuildContent(openFileDialog.FileNames);
            }
        }

        // Build Content
        private async void ButtonBuild_Click(object sender, System.EventArgs e)
        {
            await _RuntimeBuilder.BuildContent();
        }

        #region Checkmarks

        // Check Rebuild
        private void CheckBoxRebuild_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Rebuild = checkBoxRebuild.Checked;
        }

        // Check Clean
        private void CheckBoxClean_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Clean = checkBoxClean.Checked;
        }

        // Check Incremental
        private void CheckBoxIncremental_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Incremental = checkBoxIncremental.Checked;
        }

        #endregion
    }
}
