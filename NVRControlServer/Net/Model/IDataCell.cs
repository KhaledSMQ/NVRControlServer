using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model
{
    public interface IDataCell
    {
        byte[] ToBuffer();
        void FromBuffer(byte[] buffer);
    }
}
