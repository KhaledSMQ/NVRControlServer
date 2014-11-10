using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NVRControlServer.Config;

namespace NVRControlServer.UI
{
    public partial class ConfigForm : Form
    {
        private SystemXmlConfig systemXmlConfig;
        // private string systemConfigXmlFilePath = @"./Config/SystemXmlCfg.xml";//本地
        private string systemConfigXmlFilePath = @"./SystemXmlCfg.xml";//本地


        public ConfigForm()
        {
            InitializeComponent();
            systemXmlConfig = new SystemXmlConfig(systemConfigXmlFilePath);
            SystemConfigReadXmlConfig();
        }

        private void SystemConfigReadXmlConfig()
        {
            systemXmlConfig.Open();
            this.ConfigForm_NetConfigIp_textBox.Text = this.systemXmlConfig.ServerListenIp;
            this.ConfigForm_NetConfigPort_textBox.Text = this.systemXmlConfig.ServerListenPort;
            systemXmlConfig.Close();
        }

        private void ConfigForm_SaveNetConfig_button_Click(object sender, EventArgs e)
       {
           systemXmlConfig.Open();
           systemXmlConfig.ServerListenIp = "192.168.1.1";
           systemXmlConfig.ServerListenPort = "91";

         //  this.systemXmlConfig.ServerListenIp = this.ConfigForm_NetConfigIp_textBox.Text;
         //  this.systemXmlConfig.ServerListenPort = this.ConfigForm_NetConfigPort_textBox.Text;
           

           systemXmlConfig.Close();
        }
    }
}
