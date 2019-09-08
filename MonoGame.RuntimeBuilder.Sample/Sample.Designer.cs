namespace MonoGame.RuntimeBuilder.Sample
{
    partial class Sample
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.buttonPickFiles = new System.Windows.Forms.Button();
            this.flowLayoutPanelChecks = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxRebuild = new System.Windows.Forms.CheckBox();
            this.checkBoxClean = new System.Windows.Forms.CheckBox();
            this.checkBoxIncremental = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.flowLayoutPanelChecks.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.richTextBoxLog);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.buttonBuild);
            this.splitContainerMain.Panel2.Controls.Add(this.buttonPickFiles);
            this.splitContainerMain.Panel2.Controls.Add(this.flowLayoutPanelChecks);
            this.splitContainerMain.Size = new System.Drawing.Size(1420, 757);
            this.splitContainerMain.SplitterDistance = 650;
            this.splitContainerMain.TabIndex = 0;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1420, 650);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // buttonBuild
            // 
            this.buttonBuild.AutoSize = true;
            this.buttonBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonBuild.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBuild.Location = new System.Drawing.Point(0, 58);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(1420, 35);
            this.buttonBuild.TabIndex = 2;
            this.buttonBuild.Text = "Build";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.ButtonBuild_Click);
            // 
            // buttonPickFiles
            // 
            this.buttonPickFiles.AutoSize = true;
            this.buttonPickFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPickFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPickFiles.Location = new System.Drawing.Point(0, 28);
            this.buttonPickFiles.Name = "buttonPickFiles";
            this.buttonPickFiles.Size = new System.Drawing.Size(1420, 30);
            this.buttonPickFiles.TabIndex = 1;
            this.buttonPickFiles.Text = "Pick Files";
            this.buttonPickFiles.UseVisualStyleBackColor = true;
            this.buttonPickFiles.Click += new System.EventHandler(this.ButtonPickFiles_Click);
            // 
            // flowLayoutPanelChecks
            // 
            this.flowLayoutPanelChecks.AutoSize = true;
            this.flowLayoutPanelChecks.Controls.Add(this.checkBoxRebuild);
            this.flowLayoutPanelChecks.Controls.Add(this.checkBoxClean);
            this.flowLayoutPanelChecks.Controls.Add(this.checkBoxIncremental);
            this.flowLayoutPanelChecks.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelChecks.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelChecks.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelChecks.Name = "flowLayoutPanelChecks";
            this.flowLayoutPanelChecks.Size = new System.Drawing.Size(1420, 28);
            this.flowLayoutPanelChecks.TabIndex = 0;
            // 
            // checkBoxRebuild
            // 
            this.checkBoxRebuild.AutoSize = true;
            this.checkBoxRebuild.Location = new System.Drawing.Point(1338, 3);
            this.checkBoxRebuild.Name = "checkBoxRebuild";
            this.checkBoxRebuild.Size = new System.Drawing.Size(79, 22);
            this.checkBoxRebuild.TabIndex = 0;
            this.checkBoxRebuild.Text = "Rebuild";
            this.checkBoxRebuild.UseVisualStyleBackColor = true;
            this.checkBoxRebuild.CheckedChanged += new System.EventHandler(this.CheckBoxRebuild_CheckedChanged);
            // 
            // checkBoxClean
            // 
            this.checkBoxClean.AutoSize = true;
            this.checkBoxClean.Location = new System.Drawing.Point(1264, 3);
            this.checkBoxClean.Name = "checkBoxClean";
            this.checkBoxClean.Size = new System.Drawing.Size(68, 22);
            this.checkBoxClean.TabIndex = 1;
            this.checkBoxClean.Text = "Clean";
            this.checkBoxClean.UseVisualStyleBackColor = true;
            this.checkBoxClean.CheckedChanged += new System.EventHandler(this.CheckBoxClean_CheckedChanged);
            // 
            // checkBoxIncremental
            // 
            this.checkBoxIncremental.AutoSize = true;
            this.checkBoxIncremental.Location = new System.Drawing.Point(1152, 3);
            this.checkBoxIncremental.Name = "checkBoxIncremental";
            this.checkBoxIncremental.Size = new System.Drawing.Size(106, 22);
            this.checkBoxIncremental.TabIndex = 2;
            this.checkBoxIncremental.Text = "Incremental";
            this.checkBoxIncremental.UseVisualStyleBackColor = true;
            this.checkBoxIncremental.CheckedChanged += new System.EventHandler(this.CheckBoxIncremental_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            // 
            // Sample
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1420, 757);
            this.Controls.Add(this.splitContainerMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Sample";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MonoGame.RuntimeBuilder.Sample";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.flowLayoutPanelChecks.ResumeLayout(false);
            this.flowLayoutPanelChecks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelChecks;
        private System.Windows.Forms.CheckBox checkBoxRebuild;
        private System.Windows.Forms.CheckBox checkBoxClean;
        private System.Windows.Forms.CheckBox checkBoxIncremental;
        private System.Windows.Forms.Button buttonBuild;
        private System.Windows.Forms.Button buttonPickFiles;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

