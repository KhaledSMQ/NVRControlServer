using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Control;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void FileSendBufferEventHandler(object sender, FileSendBufferEventArgs e);

    public class FileSendBufferEventArgs :EventArgs
    {
        private SendFileManager sendFileManager;
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public  SendFileManager SendFileManager
        {
            get { return sendFileManager; }
            set { sendFileManager = value; }
        }

        public FileSendBufferEventArgs(SendFileManager sendFileManager, int size)
            : base()
        {
            this.sendFileManager = sendFileManager;
            this.size = size;
        }


    }
}
