using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.Event
{
    public delegate void ReadFileBufferEventHandler(object sender, ReadFileBufferEventArgs e);

    public class ReadFileBufferEventArgs : EventArgs
    {
        private int index;
        private byte[] buffer;

        public ReadFileBufferEventArgs(int index, byte[] buffer)
        {
            this.index = index;
            this.buffer = buffer;
        }

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

    }
}
