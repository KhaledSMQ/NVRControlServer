using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NVRControlServer.UI
{
    public partial class SearchVideoFile : Form
    {
        public SearchVideoFile()
        {
            InitializeComponent();
        }




        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            public char szDisplayName;
            public char szTypeName;
        }


        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
