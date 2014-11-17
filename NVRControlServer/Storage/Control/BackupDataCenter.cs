using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.NVR.Control;
using System.Windows.Forms;

using NVRControlServer.Storage.Utils;
using NVRControlServer.DataBase.Dao;
using NVRControlServer.Storage.Model;
using NVRControlServer.Storage.Module;
using System.Threading;

namespace NVRControlServer.Storage.Control
{
    public class BackupDataCenter 
    {

        #region 1. 变量属性

        #region 1.1 变量
        private string backupDataPath = "e:/";                                                              //存储硬盘目录
        private BackupTaskCenter taskCenter;                                                             //存储任务控制中心
        private List<NVRControler> nvrList;                                                                  //保存所有Nvr设备 
        private string autoBackupfigFile = Application.StartupPath + "\\BackupSet.ini";//定义要读取的INI文件
        private DataOperate dataoperate;
        private Thread backupWorkThread;
        private bool backupWorkFlag = false;
        #endregion 1.1 变量

        #region 1.2 属性


        #endregion 1.2 属性
 
        #endregion 1. 变量属性

        public BackupDataCenter(string backupDataPath,
            List<NVRControler> nvrList, string autoBackupfigFile)
        {
            this.backupDataPath = backupDataPath;
            this.nvrList = nvrList;
            this.autoBackupfigFile = autoBackupfigFile;
            dataoperate = new DataOperate();
        }

        public bool Initalization()
        {
            taskCenter = new BackupTaskCenter();
            if (!FileHelper.CreateNvrFoleder(backupDataPath, nvrList))
                return false;
            #region 写入配置
            //DataOperate.WritePrivateProfileString("BackupSet", "BackupVideoLength", "15", autoBackupfigFile);
            //for (int i = 0; i < nvrList.Count; i++)
            //{
            //    NVRChannel[] channels = nvrList[i].NvrChannels;
            //    for (int j = 0; j < channels.Length; j++)
            //    {
            //        //DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //        //    channels[j].ChannelIndex.ToString() + "startTime", "20140715083040", autoBackupfigFile);
            //        //DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //        //   channels[j].ChannelIndex.ToString() + "backupTime", "20140715083040", autoBackupfigFile);
            //        //DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //        //   channels[j].ChannelIndex.ToString() + "lastTime", "20140715083040", autoBackupfigFile);
            //        DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //           channels[j].ChannelIndex.ToString() + "startTime", TimeHelper.DateTimeToString(new DateTime(2014, 9, 28, 12, 20, 30)), autoBackupfigFile);
            //        DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //           channels[j].ChannelIndex.ToString() + "backupTime", TimeHelper.DateTimeToString(new DateTime(2014, 11, 3, 19, 47, 58)), autoBackupfigFile);
            //        DataOperate.WritePrivateProfileString(nvrList[i].NvrName,
            //           channels[j].ChannelIndex.ToString() + "lastTime", TimeHelper.DateTimeToString(new DateTime(2014, 11, 3, 20, 20, 30)), autoBackupfigFile);
            //    }
            //}
            #endregion 写入配置

            StartBackup();
            return true;
        }

        public void CheckNvrStatus()
        {
            
        }

        public void StartBackup()
        {
            backupWorkThread = new Thread(WorkFunc);
            backupWorkFlag = true;
            backupWorkThread.Start();
        }

        private void WorkFunc()
        {
            int backupVideoLength = Convert.ToInt32(
                dataoperate.ReadString("BackupSet", 
                "BackupVideoLength", "", autoBackupfigFile));

            while (backupWorkFlag)
            {
                for (int i = 0; i < nvrList.Count; i++)
                {
                    NVRControler nvrcontroler = nvrList[i];
                    NVRChannel[] nvrChannels = nvrcontroler.NvrChannels;

                    for (int j = 0; j < nvrChannels.Length; j++)
                    {
                    DateTime startTime = TimeHelper.StringToDateTime(
                        dataoperate.ReadString(nvrList[i].NvrName, nvrChannels[j].ChannelIndex.ToString() + "startTime", "", autoBackupfigFile));
                    DateTime lastTime = TimeHelper.StringToDateTime(
                        dataoperate.ReadString(nvrList[i].NvrName, nvrChannels[j].ChannelIndex.ToString() + "lastTime", "", autoBackupfigFile));
                    DateTime backupTime = TimeHelper.StringToDateTime(
                        dataoperate.ReadString(nvrList[i].NvrName, nvrChannels[j].ChannelIndex.ToString() + "backupTime", "", autoBackupfigFile));

                    //DateTime startTime = TimeHelper.StringToDateTime(
                    //   dataoperate.ReadString(nvrList[0].NvrName, nvrChannels[0].ChannelIndex.ToString() + "startTime", "", autoBackupfigFile));
                    //DateTime lastTime = TimeHelper.StringToDateTime(
                    //    dataoperate.ReadString(nvrList[0].NvrName, nvrChannels[0].ChannelIndex.ToString() + "lastTime", "", autoBackupfigFile));
                    //DateTime backupTime = TimeHelper.StringToDateTime(
                    //    dataoperate.ReadString(nvrList[0].NvrName, nvrChannels[0].ChannelIndex.ToString() + "backupTime", "", autoBackupfigFile));


                        //if (backupTime < lastTime && backupTime >= startTime)
                        //{
                        string channelBackupStorageDataPath = backupDataPath +
                        nvrcontroler.NvrName + "/" + 0 + "/";
                        BuckupTaskParameter taskParameter =
                            new BuckupTaskParameter(0, 0, backupVideoLength, 2, nvrcontroler,
                                channelBackupStorageDataPath, new TimeSpan(0, backupVideoLength, 0), backupTime,
                                backupTime + new TimeSpan(0, backupVideoLength, 0), autoBackupfigFile);
                        Console.WriteLine("备份任务的开始时间:" + TimeHelper.DateTimeToString(backupTime));
                        BackupTask backupTask = new BackupTask(taskParameter);
                        backupTask.Start();
                        
                        //}
                        //else if (backupTime >= lastTime)
                        //{
                        //    break;
                        //}
                        // }
                    }
                }
            }
        }



        public void StopBackup()
        {
           
        }

        public void StartAllTask()
        {
            
        }


        public void StopAllTask()
        {
            
        }
    }
}
