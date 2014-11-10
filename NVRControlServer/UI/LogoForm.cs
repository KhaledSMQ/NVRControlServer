using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace NVRControlServer.UI
{
    public partial class LogoForm : Office2007RibbonForm
    {

        public LogoForm()
        {
            InitializeComponent();
            this.LogProgressBar.Value = 0;
            this.LogProgressBar.IsRunning = true;

            this.Show();
        }

        private void LoginTimer_Tick(object sender, EventArgs e)
        {
            this.LogProgressBar.Value += 7;
            if (this.LogProgressBar.Value >= 100)
                this.Close();
        }
    }
}
