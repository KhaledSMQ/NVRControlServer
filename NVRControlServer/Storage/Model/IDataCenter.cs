using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Storage.Control
{
    public interface IDataCenter
    {
        Boolean Initalization();
        void CheckNvrStatus();
        void StartBackup();
        void StopBackup();
        void StartAllTask();
        void StopAllTask();
    }
}
