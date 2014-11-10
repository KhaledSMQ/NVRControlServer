namespace NVRControlServer.UI
{
    partial class LogoForm
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
            this.LoginTimer = new System.Windows.Forms.Timer(this.components);
            this.ApplicationLabel = new System.Windows.Forms.Label();
            this.LogProgressBar = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoginTimer
            // 
            this.LoginTimer.Enabled = true;
            this.LoginTimer.Tick += new System.EventHandler(this.LoginTimer_Tick);
            // 
            // ApplicationLabel
            // 
            this.ApplicationLabel.AutoSize = true;
            this.ApplicationLabel.BackColor = System.Drawing.Color.Black;
            this.ApplicationLabel.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ApplicationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ApplicationLabel.Location = new System.Drawing.Point(202, 183);
            this.ApplicationLabel.Name = "ApplicationLabel";
            this.ApplicationLabel.Size = new System.Drawing.Size(159, 28);
            this.ApplicationLabel.TabIndex = 0;
            this.ApplicationLabel.Text = "云台控制服务器";
            // 
            // LogProgressBar
            // 
            this.LogProgressBar.BackColor = System.Drawing.Color.Silver;
            // 
            // 
            // 
            this.LogProgressBar.BackgroundStyle.Class = "";
            this.LogProgressBar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.LogProgressBar.Location = new System.Drawing.Point(198, 217);
            this.LogProgressBar.Name = "LogProgressBar";
            this.LogProgressBar.PieBorderDark = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LogProgressBar.ProgressBarType = DevComponents.DotNetBar.eCircularProgressType.Dot;
            this.LogProgressBar.ProgressColor = System.Drawing.Color.White;
            this.LogProgressBar.ProgressText = "0";
            this.LogProgressBar.ProgressTextVisible = true;
            this.LogProgressBar.Size = new System.Drawing.Size(76, 42);
            this.LogProgressBar.SpokeBorderDark = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LogProgressBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.LogProgressBar.TabIndex = 1;
            this.LogProgressBar.TabStop = false;
            this.LogProgressBar.Tag = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(275, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "正在初始化....";
            // 
            // LogoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NVRControlServer.Properties.Resources.Logo;
            this.ClientSize = new System.Drawing.Size(561, 353);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LogProgressBar);
            this.Controls.Add(this.ApplicationLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer LoginTimer;
        private System.Windows.Forms.Label ApplicationLabel;
        private DevComponents.DotNetBar.Controls.CircularProgress LogProgressBar;
        private System.Windows.Forms.Label label1;
    }
}