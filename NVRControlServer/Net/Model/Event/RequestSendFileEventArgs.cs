using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void RequestSendFileEventHandler(object sender, RequestSendFileEventArgs e);

    public class RequestSendFileEventArgs : CancelEventArgs
    {
        private TraFransfersFileStart transferFileStart;
        private TcpPort transTcpPort;
        private string savingpath;

        public TraFransfersFileStart TransferFileStart
        {
            get { return transferFileStart; }
            set { transferFileStart = value; }
        }

        public TcpPort TransTcpPort
        {
            get { return transTcpPort; }
            set { transTcpPort = value; }
        }

        public string Savingpath
        {
            get { return savingpath; }
            set { savingpath = value; }
        }

        public RequestSendFileEventArgs(
            TraFransfersFileStart transferFileStart,
            TcpPort transTcpPort)
            :base()
        {
            this.transferFileStart = transferFileStart;
            this.transTcpPort = transTcpPort;
        }

        public RequestSendFileEventArgs(
            TraFransfersFileStart transferFileStart,
            TcpPort transTcpPort, bool cancel)
            : base(cancel)
        {
            this.transferFileStart = transferFileStart;
            this.transTcpPort = transTcpPort;
        }


    }
}
