using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Interface
{
    public interface IDataFrame
    {
        byte[] ToBuffer();
        void FromBuffer(byte[] buffer);
    }
}
