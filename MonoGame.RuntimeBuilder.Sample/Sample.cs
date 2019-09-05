using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Windows.Forms;

namespace MonoGame.RuntimeBuilder.Sample
{
    public partial class Sample : Form
    {
        private RuntimeBuilder _RuntimeBuilder { get; set; }

        public Sample()
        {
            InitializeComponent();

            _RuntimeBuilder = new RuntimeBuilder(
                Path.Combine(Application.StartupPath, "working"),
                Path.Combine(Application.StartupPath, "working", "obj"),
                Path.Combine(Application.StartupPath, "Content", "Generated"),
                TargetPlatform.Windows,
                GraphicsProfile.Reach,
                true)
            {
                Logger = new StringBuilderLogger()
            };

            ((StringBuilderLogger)_RuntimeBuilder.Logger).OnMessageLogged += (log) =>
            {
                richTextBoxLog.Text = log;
                richTextBoxLog.SelectionStart = richTextBoxLog.Text.Length;
                richTextBoxLog.ScrollToCaret();
            };
        }

        // Pick Files & Build Content
        private async void buttonPickFiles_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                await _RuntimeBuilder.BuildContent(openFileDialog.FileNames);
            }
        }

        // Build Content
        private async void buttonBuild_Click(object sender, System.EventArgs e)
        {
            await _RuntimeBuilder.BuildContent();
        }

        #region Checkmarks

        // Check Rebuild
        private void checkBoxRebuild_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Rebuild = checkBoxRebuild.Checked;
        }

        // Check Clean
        private void checkBoxClean_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Clean = checkBoxClean.Checked;
        }

        // Check Incremental
        private void checkBoxIncremental_CheckedChanged(object sender, System.EventArgs e)
        {
            _RuntimeBuilder.Incremental = checkBoxIncremental.Checked;
        }

        #endregion
    }
}
