using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void TcpPortReceiveDataEventHandler(object o, TcpPortReceiveDataEventArgs e);

    public class TcpPortReceiveDataEventArgs :EventArgs
    {
        private TcpPort receiveTcpPort;

        public TcpPort ReceiveTcpPort
        {
            get { return receiveTcpPort; }
            set { receiveTcpPort = value; }
        }
        private byte[] receiveData;

        public byte[] ReceiveData
        {
            get { return receiveData; }
            set { receiveData = value; }
        }

        public TcpPortReceiveDataEventArgs(TcpPort receiveTcpPort, byte[] receiveData)
        {
            this.receiveTcpPort = receiveTcpPort;
            this.receiveData = receiveData;
        }

    }
}
