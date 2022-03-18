namespace gather_standalone
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.MainTitle = new System.Windows.Forms.Label();
            this.IdTitleLabel = new System.Windows.Forms.Label();
            this.IdLabel = new System.Windows.Forms.LinkLabel();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.HideButton = new System.Windows.Forms.Button();
            this.QuitOnLeaveOption = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // MainTitle
            // 
            this.MainTitle.AutoSize = true;
            this.MainTitle.Location = new System.Drawing.Point(22, 21);
            this.MainTitle.Name = "MainTitle";
            this.MainTitle.Size = new System.Drawing.Size(828, 25);
            this.MainTitle.TabIndex = 0;
            this.MainTitle.Text = "Keep this running to send current Kills/Deaths of the server your playing to game" +
    "tools";
            // 
            // IdTitleLabel
            // 
            this.IdTitleLabel.AutoSize = true;
            this.IdTitleLabel.Location = new System.Drawing.Point(22, 46);
            this.IdTitleLabel.Name = "IdTitleLabel";
            this.IdTitleLabel.Size = new System.Drawing.Size(233, 25);
            this.IdTitleLabel.TabIndex = 3;
            this.IdTitleLabel.Text = "Your unique sender ID:";
            // 
            // IdLabel
            // 
            this.IdLabel.AutoSize = true;
            this.IdLabel.Location = new System.Drawing.Point(252, 46);
            this.IdLabel.Name = "IdLabel";
            this.IdLabel.Size = new System.Drawing.Size(424, 25);
            this.IdLabel.TabIndex = 5;
            this.IdLabel.TabStop = true;
            this.IdLabel.Text = "00000000-0000-0000-0000-000000000000";
            this.IdLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.IdLabel_LinkClicked);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.InfoLabel.Location = new System.Drawing.Point(675, 46);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(175, 25);
            this.InfoLabel.TabIndex = 6;
            this.InfoLabel.Text = "(Click ID to copy)";
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayIcon.BalloonTipText = "Battlefield 1 Playerlist sender has minimized to the system tray, doubleclick the" +
    " icon to reopen.";
            this.TrayIcon.BalloonTipTitle = "Battlefield 1 Playerlist sender";
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "Battlefield 1 Playerlist sender";
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // HideButton
            // 
            this.HideButton.Location = new System.Drawing.Point(607, 81);
            this.HideButton.Name = "HideButton";
            this.HideButton.Size = new System.Drawing.Size(239, 40);
            this.HideButton.TabIndex = 7;
            this.HideButton.Text = "Hide in system tray";
            this.HideButton.UseVisualStyleBackColor = true;
            this.HideButton.Click += new System.EventHandler(this.HideButton_Click);
            // 
            // QuitOnLeaveOption
            // 
            this.QuitOnLeaveOption.AutoSize = true;
            this.QuitOnLeaveOption.Location = new System.Drawing.Point(27, 85);
            this.QuitOnLeaveOption.Name = "QuitOnLeaveOption";
            this.QuitOnLeaveOption.Size = new System.Drawing.Size(352, 29);
            this.QuitOnLeaveOption.TabIndex = 8;
            this.QuitOnLeaveOption.Text = "Quit if Battlefield 1 isn\'t detected";
            this.QuitOnLeaveOption.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(879, 140);
            this.Controls.Add(this.QuitOnLeaveOption);
            this.Controls.Add(this.HideButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.IdLabel);
            this.Controls.Add(this.IdTitleLabel);
            this.Controls.Add(this.MainTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Battlefield 1 Playerlist sender";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MainTitle;
        private System.Windows.Forms.Label IdTitleLabel;
        private System.Windows.Forms.LinkLabel IdLabel;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.Button HideButton;
        private System.Windows.Forms.CheckBox QuitOnLeaveOption;
    }
}

