using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using NVRControlServer.Storage.Model;

namespace NVRControlServer.Storage.Control
{
    public class BackupTaskCenter : ITaskCente
    {
        private ArrayList taskList;

        public ArrayList TaskList
        {
            get { return this.taskList; }
        }

        public BackupTaskCenter()
        {
            taskList = new ArrayList();
        }

        public void AddTask(ITask task)
        {
            taskList.Add(task);
        }

        public void DelTask(ITask task)
        {
            taskList.Remove(task);
        }

        public void StartAllTask()
        {

        }

        public void StopAllTask()
        {

        }

        public void StartTask(ITask task)
        {

        }

        public void StopTask(ITask task)
        {
            
        }
    }
}
