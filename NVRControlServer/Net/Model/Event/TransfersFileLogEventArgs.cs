using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void TransfersFileLogEventHandler(object sender, TransfersFileLogEventArgs e);

    public class TransfersFileLogEventArgs : EventArgs
    {
        private String message;

        public TransfersFileLogEventArgs(String message)
            :base()
        {
            this.message = message;
        }

        public String Message
        {
            get { return message; }
            set { message = value; }
        }


    }
}
