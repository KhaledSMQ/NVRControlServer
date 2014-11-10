using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.Storage.Model;

namespace NVRControlServer.Storage.Control
{
    public interface ITaskCente
    {
        void AddTask(ITask task);
        void DelTask(ITask task);
        void StartAllTask();
        void StopAllTask();
        void StartTask(ITask task);
        void StopTask(ITask task);
    }
}
