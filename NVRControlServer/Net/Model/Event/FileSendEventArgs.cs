using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Control;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void FileSendEventHandler(object sender, FileSendEventArgs e);

    public class FileSendEventArgs : EventArgs
    {
        private SendFileManager sendFileManager;

        public FileSendEventArgs(SendFileManager sendFileManager)
            : base()
        {
            this.sendFileManager = sendFileManager;
        }

        internal SendFileManager SendFileManager
        {
            get { return sendFileManager; }
            set { sendFileManager = value; }
        }
    }
}
