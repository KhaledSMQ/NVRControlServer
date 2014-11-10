using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NVRControlServer.Net.Model
{
    public delegate void ReceiveDataEventHandler(object sender, ReceiveDataEventArgs e);

    public class ReceiveDataEventArgs : EventArgs
    {
        private byte[] buffer;
        private IPEndPoint remoteIp;

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public IPEndPoint RemoteIp
        {
            get { return remoteIp; }
            set { remoteIp = value; }
        }

        public ReceiveDataEventArgs()
        {
        }

        public ReceiveDataEventArgs(byte[] buffer, IPEndPoint remoteIp)
            :base()
        {
            this.buffer = buffer;
            this.remoteIp = remoteIp;
        }


    }
}
