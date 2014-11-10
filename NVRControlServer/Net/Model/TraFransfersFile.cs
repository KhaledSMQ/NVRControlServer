using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model
{
    [Serializable]
    public class TraFransfersFile
    {
        private string md5;
        private int index;
        private byte[] buffer;

        public string Md5
        {
            get { return md5; }
            set { md5 = value; }
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

        public TraFransfersFile(string md5,int index, byte[] buffer)
        {
            this.md5 = md5;
            this.index = index;
            this.buffer = buffer;
        }

    }
}
