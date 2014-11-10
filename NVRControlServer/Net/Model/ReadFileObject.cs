using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net
{
    internal class ReadFileObject
    {
        private int index;
        private byte[] buffer;

        public ReadFileObject(int index, byte[] buffer)
        {
            this.index = index;
            this.buffer = buffer;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

    }

}
