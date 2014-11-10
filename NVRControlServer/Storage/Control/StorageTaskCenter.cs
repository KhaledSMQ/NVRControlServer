#region ************************文件说明************************************
/// 作者(Author)：                     黄顺彬
/// 
/// 日期(Create Date)：            2014.6.17
/// 
/// 功能：                                    存储任务中心 
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using NVRControlServer.Storage.Module;

namespace NVRControlServer.Storage.Control
{
    public class StorageTaskCenter
    {
        #region 1.变量属性
        
        #region 1.1 变量
        private ArrayList m_sheduleTasks;  // 任务列表
        #endregion 1.1 变量

        #region 1.2属性
        ArrayList ScheduleTasks
        {
            get
            {
                return m_sheduleTasks;
            }
        }
        #endregion 1.2属性

        #endregion 1.变量属性

        #region 2.构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StorageTaskCenter()
        {
            m_sheduleTasks = new ArrayList();
        }
        #endregion 2.构造函数

        #region 3.公有方法
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="newTask"></param>
        public void AddTask(StorageTask newTask)
        {
            m_sheduleTasks.Add(newTask);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="delTask"></param>
        public void DelTask(StorageTask delTask)
        {
            m_sheduleTasks.Remove(delTask);
        }

        /// <summary>
        /// 启动所有任务
        /// </summary>
        public void StartAllTask()
        {
            foreach (StorageTask task in ScheduleTasks)
            {
                StartTask(task);
            }
        }

        /// <summary>
        /// 启动一个任务
        /// </summary>
        /// <param name="task"></param>
        public void StartTask(StorageTask task)
        {
            if (task.Job == null)
            {
                task.Job += new TimerCallback(task.Execute);
            }
            task.Start();
        }

        public void StopAllTask()
        {
            foreach (StorageTask task in ScheduleTasks)
            {
                
            }

        }

        /// <summary>
        /// 终止所有的任务
        /// </summary>
        public void TerminateAllTask()
        {
            foreach (StorageTask task in ScheduleTasks)
            {
                TerminateTask(task);
            }
            ScheduleTasks.Clear();
        }

        /// <summary>
        /// 终止一个任务
        /// </summary>
        /// <param name="task"></param>
        public void TerminateTask(StorageTask task)
        {
            task.Stop();
        }
        #endregion 3.公有方法
    }
}
