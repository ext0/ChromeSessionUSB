namespace ChromeSessionUSB
{
    partial class ChromeSession
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChromeSession));
            this.SaveSessionButton = new System.Windows.Forms.Button();
            this.RestoreSessionButton = new System.Windows.Forms.Button();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedEncryptionSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setChromePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectDirectoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveSessionButton
            // 
            this.SaveSessionButton.Location = new System.Drawing.Point(12, 31);
            this.SaveSessionButton.Name = "SaveSessionButton";
            this.SaveSessionButton.Size = new System.Drawing.Size(168, 41);
            this.SaveSessionButton.TabIndex = 0;
            this.SaveSessionButton.Text = "Save Session";
            this.SaveSessionButton.UseVisualStyleBackColor = true;
            this.SaveSessionButton.Click += new System.EventHandler(this.SaveSessionButton_Click);
            // 
            // RestoreSessionButton
            // 
            this.RestoreSessionButton.Location = new System.Drawing.Point(186, 31);
            this.RestoreSessionButton.Name = "RestoreSessionButton";
            this.RestoreSessionButton.Size = new System.Drawing.Size(163, 41);
            this.RestoreSessionButton.TabIndex = 1;
            this.RestoreSessionButton.Text = "Restore Session";
            this.RestoreSessionButton.UseVisualStyleBackColor = true;
            this.RestoreSessionButton.Click += new System.EventHandler(this.RestoreSessionButton_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(355, 28);
            this.MenuStrip.TabIndex = 2;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advancedEncryptionSettingsToolStripMenuItem,
            this.setChromePathToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // advancedEncryptionSettingsToolStripMenuItem
            // 
            this.advancedEncryptionSettingsToolStripMenuItem.Name = "advancedEncryptionSettingsToolStripMenuItem";
            this.advancedEncryptionSettingsToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.advancedEncryptionSettingsToolStripMenuItem.Text = "Advanced encryption settings";
            // 
            // setChromePathToolStripMenuItem
            // 
            this.setChromePathToolStripMenuItem.Name = "setChromePathToolStripMenuItem";
            this.setChromePathToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.setChromePathToolStripMenuItem.Text = "Set Chrome path";
            this.setChromePathToolStripMenuItem.Click += new System.EventHandler(this.setChromePathToolStripMenuItem_Click);
            // 
            // ChromeSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 80);
            this.Controls.Add(this.RestoreSessionButton);
            this.Controls.Add(this.SaveSessionButton);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.MaximizeBox = false;
            this.Name = "ChromeSession";
            this.Text = "USB Chrome Session";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveSessionButton;
        private System.Windows.Forms.Button RestoreSessionButton;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedEncryptionSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setChromePathToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog selectDirectoryDialog;
    }
}

