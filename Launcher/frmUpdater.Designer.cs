namespace Intersect_Updater
{
    partial class frmUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdater));
            this.lblStatus = new System.Windows.Forms.Label();
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.tmrChecking = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.percentLabel = new System.Windows.Forms.Label();
            this.icon_discord = new System.Windows.Forms.PictureBox();
            this.icon_jkhub = new System.Windows.Forms.PictureBox();
            this.icon_moddb = new System.Windows.Forms.PictureBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonClose = new System.Windows.Forms.PictureBox();
            this.buttonMinimize = new System.Windows.Forms.PictureBox();
            this.WindowMovePanel = new TransparentPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_discord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_jkhub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_moddb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonMinimize)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblStatus.Location = new System.Drawing.Point(3, 372);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(729, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Please wait, checking for updates";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBackground
            // 
            this.picBackground.BackColor = System.Drawing.Color.Black;
            this.picBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBackground.Location = new System.Drawing.Point(0, 0);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(732, 425);
            this.picBackground.TabIndex = 2;
            this.picBackground.TabStop = false;
            this.picBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.picBackground_Paint);
            // 
            // tmrChecking
            // 
            this.tmrChecking.Enabled = true;
            this.tmrChecking.Interval = 333;
            this.tmrChecking.Tick += new System.EventHandler(this.tmrChecking_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.progressBar1.Location = new System.Drawing.Point(0, 390);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(732, 16);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 100;
            // 
            // percentLabel
            // 
            this.percentLabel.BackColor = System.Drawing.Color.Transparent;
            this.percentLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.percentLabel.Location = new System.Drawing.Point(0, 409);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(732, 16);
            this.percentLabel.TabIndex = 0;
            this.percentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.percentLabel.Visible = false;
            // 
            // icon_discord
            // 
            this.icon_discord.BackColor = System.Drawing.Color.Transparent;
            this.icon_discord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.icon_discord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.icon_discord.Image = ((System.Drawing.Image)(resources.GetObject("icon_discord.Image")));
            this.icon_discord.ImageLocation = "";
            this.icon_discord.InitialImage = ((System.Drawing.Image)(resources.GetObject("icon_discord.InitialImage")));
            this.icon_discord.Location = new System.Drawing.Point(37, 311);
            this.icon_discord.Name = "icon_discord";
            this.icon_discord.Size = new System.Drawing.Size(76, 47);
            this.icon_discord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.icon_discord.TabIndex = 3;
            this.icon_discord.TabStop = false;
            this.icon_discord.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // icon_jkhub
            // 
            this.icon_jkhub.BackColor = System.Drawing.Color.Transparent;
            this.icon_jkhub.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.icon_jkhub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.icon_jkhub.Image = ((System.Drawing.Image)(resources.GetObject("icon_jkhub.Image")));
            this.icon_jkhub.InitialImage = ((System.Drawing.Image)(resources.GetObject("icon_jkhub.InitialImage")));
            this.icon_jkhub.Location = new System.Drawing.Point(133, 311);
            this.icon_jkhub.Name = "icon_jkhub";
            this.icon_jkhub.Size = new System.Drawing.Size(76, 47);
            this.icon_jkhub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.icon_jkhub.TabIndex = 4;
            this.icon_jkhub.TabStop = false;
            this.icon_jkhub.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // icon_moddb
            // 
            this.icon_moddb.BackColor = System.Drawing.Color.Transparent;
            this.icon_moddb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.icon_moddb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.icon_moddb.Image = ((System.Drawing.Image)(resources.GetObject("icon_moddb.Image")));
            this.icon_moddb.InitialImage = ((System.Drawing.Image)(resources.GetObject("icon_moddb.InitialImage")));
            this.icon_moddb.Location = new System.Drawing.Point(227, 311);
            this.icon_moddb.Name = "icon_moddb";
            this.icon_moddb.Size = new System.Drawing.Size(76, 47);
            this.icon_moddb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.icon_moddb.TabIndex = 5;
            this.icon_moddb.TabStop = false;
            this.icon_moddb.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "Minimized to system tray.";
            this.notifyIcon.BalloonTipTitle = "Star Wars: Warzone - Launcher";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Star Wars: Warzone - Launcher";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonClose.Image = ((System.Drawing.Image)(resources.GetObject("buttonClose.Image")));
            this.buttonClose.InitialImage = ((System.Drawing.Image)(resources.GetObject("buttonClose.InitialImage")));
            this.buttonClose.Location = new System.Drawing.Point(680, 0);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(52, 23);
            this.buttonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.buttonClose.TabIndex = 10;
            this.buttonClose.TabStop = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            this.buttonClose.MouseEnter += new System.EventHandler(this.buttonClose_MouseHover);
            this.buttonClose.MouseLeave += new System.EventHandler(this.buttonClose_MouseLeave);
            this.buttonClose.MouseHover += new System.EventHandler(this.buttonClose_MouseHover);
            // 
            // buttonMinimize
            // 
            this.buttonMinimize.BackColor = System.Drawing.Color.Transparent;
            this.buttonMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonMinimize.Image = ((System.Drawing.Image)(resources.GetObject("buttonMinimize.Image")));
            this.buttonMinimize.InitialImage = ((System.Drawing.Image)(resources.GetObject("buttonMinimize.InitialImage")));
            this.buttonMinimize.Location = new System.Drawing.Point(646, 0);
            this.buttonMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMinimize.Name = "buttonMinimize";
            this.buttonMinimize.Size = new System.Drawing.Size(34, 23);
            this.buttonMinimize.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.buttonMinimize.TabIndex = 11;
            this.buttonMinimize.TabStop = false;
            this.buttonMinimize.Click += new System.EventHandler(this.buttonMinimize_Click);
            this.buttonMinimize.MouseEnter += new System.EventHandler(this.buttonMinimize_MouseHover);
            this.buttonMinimize.MouseLeave += new System.EventHandler(this.buttonMinimize_MouseLeave);
            this.buttonMinimize.MouseHover += new System.EventHandler(this.buttonMinimize_MouseHover);
            // 
            // WindowMovePanel
            // 
            this.WindowMovePanel.Location = new System.Drawing.Point(3, 0);
            this.WindowMovePanel.Name = "WindowMovePanel";
            this.WindowMovePanel.Opacity = 0;
            this.WindowMovePanel.Size = new System.Drawing.Size(586, 37);
            this.WindowMovePanel.TabIndex = 9;
            this.WindowMovePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMovePanel_MouseDown);
            this.WindowMovePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMovePanel_MouseMove);
            this.WindowMovePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WindowMovePanel_MouseUp);
            // 
            // frmUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(732, 425);
            this.Controls.Add(this.buttonMinimize);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.WindowMovePanel);
            this.Controls.Add(this.icon_moddb);
            this.Controls.Add(this.icon_jkhub);
            this.Controls.Add(this.icon_discord);
            this.Controls.Add(this.percentLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.picBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmUpdater_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_discord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_jkhub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icon_moddb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonMinimize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.Timer tmrChecking;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label percentLabel;
        private System.Windows.Forms.PictureBox icon_discord;
        private System.Windows.Forms.PictureBox icon_jkhub;
        private System.Windows.Forms.PictureBox icon_moddb;
        private TransparentPanel WindowMovePanel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.PictureBox buttonClose;
        private System.Windows.Forms.PictureBox buttonMinimize;
    }
}

