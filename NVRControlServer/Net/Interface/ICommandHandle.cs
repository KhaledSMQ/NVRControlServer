using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Model;

namespace NVRControlServer.Net.Interface
{
    public interface ICommandHandle
    {
        void SetHandleModel(ClientSession session, ClientCommand command);
        void SetNextHandler(ICommandHandle nextHandle);
        void  Process();
        
    }
}
