﻿namespace Intersect_Updater
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.tmrChecking = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.percentLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
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
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // picBackground
            // 
            this.picBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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
            this.percentLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.percentLabel.Visible = false;
            // 
            // frmUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(732, 425);
            this.Controls.Add(this.percentLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.picBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmUpdater_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.Timer tmrChecking;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label percentLabel;
    }
}

