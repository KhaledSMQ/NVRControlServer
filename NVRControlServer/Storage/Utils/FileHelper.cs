#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：            2014.6.11
/// 
/// 功能：                                    文件操作类
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using NVRControlServer.NVR.Control;
using NVRControlServer.Utils.Log;

namespace NVRControlServer.Storage.Utils
{
    [Serializable]
    public struct SearchVideoFileInfo
    {
        private string videoFileName;

        public string VideoFileName
        {
            get { return videoFileName; }
            set { videoFileName = value; }
        }
        private string videoFileLength;

        public string VideoFileLength
        {
            get { return videoFileLength; }
            set { videoFileLength = value; }
        }
        private DateTime videoFileStartTime;

        public DateTime VideoFileStartTime
        {
            get { return videoFileStartTime; }
            set { videoFileStartTime = value; }
        }
        private DateTime videoFileEndTime;

        public DateTime VideoFileEndTime
        {
            get { return videoFileEndTime; }
            set { videoFileEndTime = value; }
        }

        public SearchVideoFileInfo(string videoFileName, string videoFileLength,
            DateTime videoFileStartTime, DateTime videoFileEndTime)
        {
            this.videoFileName = videoFileName;
            this.videoFileLength = videoFileLength;
            this.videoFileStartTime = videoFileStartTime;
            this.videoFileEndTime = videoFileEndTime;
        }


    }
    

    public class FileHelper 
    {
        private bool alreadyDispose = false;

        public FileHelper()
        {
        }

        ~FileHelper()
        {

        }

        /// <summary>
        /// 在但前面目录下创建目录
        /// </summary>
        /// <param name="orignFolder">当前目录</param>
        /// <param name="NewFolder">新目录</param>
        public static void CreateFolder(string orignFolder, string NewFolder)
        {
            Directory.SetCurrentDirectory(orignFolder);
            Directory.CreateDirectory(NewFolder);
        }


        public static string CreateFolder(string orignFolder, DateTime nowTime)
        {
            int mYear = nowTime.Year;
            int mMonth = nowTime.Month;
            int mDay = nowTime.Day;
            int mHour = nowTime.Hour;

            Directory.CreateDirectory(orignFolder + "/" + mYear);
            Directory.CreateDirectory(orignFolder + "/" + mYear + "/" + mMonth);
            Directory.CreateDirectory(orignFolder + "/" + mYear + "/" + mMonth + "/" + mDay);
            Directory.CreateDirectory(orignFolder + "/" + mYear + "/" + mMonth + "/" + mDay + "/" + mHour);

            return orignFolder + "/" + mYear + "/" + mMonth + "/" + mDay + "/" + mHour;

        }


        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir">删除目录</param>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d);
                    else
                        DeleteFolder(d);
                }
                Directory.Delete(dir);
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        public static void CopyDir(string srcPath, string dstPath)
        {
            try
            {
                if (dstPath[dstPath.Length - 1] != Path.DirectorySeparatorChar)
                    dstPath += Path.DirectorySeparatorChar;
                if (!Directory.Exists(dstPath))
                    Directory.CreateDirectory(dstPath);
                string[] mFileList = Directory.GetFileSystemEntries(srcPath);

                foreach (string file in mFileList)
                {
                    if (Directory.Exists(file))
                        CopyDir(file, dstPath + Path.GetFileName(file));
                    else
                        File.Copy(file, dstPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 创建路径
        /// </summary>
        /// <param name="newFolder">新路径名</param>
        /// <returns></returns>
        public static bool CreateFolder(string newFolder)
        {
            Directory.CreateDirectory(newFolder);
            return true;
        }

        
        public static bool CreateNvrFoleder(string currentPath,string[] nvrInfo, int maxChannel)
        {
            if (CreateFolder(currentPath))
            {
                Directory.SetCurrentDirectory(currentPath);
            }

            try
            {
                for (int i = 0; i < nvrInfo.Length; i++)
                {
                    if (CreateFolder(nvrInfo[i]))
                    {
                        Directory.SetCurrentDirectory(currentPath + '/' + nvrInfo[i]);
                        DirectoryInfo mDirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
                        string pathName = mDirInfo.FullName;
                        for (int j = 0; j < maxChannel; j++)
                        {
                            CreateFolder(j.ToString());
                        }
                    }
                    Directory.SetCurrentDirectory(currentPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        public static bool CreateNvrFoleder(string currentPath, List<NVRControler> nvrList)
        {
            if (CreateFolder(currentPath))
            {
                Directory.SetCurrentDirectory(currentPath);
            }

            try
            {
                for (int i = 0; i < nvrList.Count; i++)
                {
                    if (CreateFolder(nvrList[i].NvrName))
                    {
                        Directory.SetCurrentDirectory(currentPath + '/' + nvrList[i].NvrName);
                        DirectoryInfo mDirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
                        string pathName = mDirInfo.FullName;
                        for (int j = 0; j < nvrList[i].NvrMaxChannelNum; j++)
                        {
                            CreateFolder(j.ToString());
                        }
                    }
                    Directory.SetCurrentDirectory(currentPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }


 
        public static string getDateTime()
        {
            return DateTime.Today.ToString();
        }


        /// <summary>
        /// 返回保存视频文件名
        /// </summary>
        /// <param name="nvrSelectedIndex">nvr编号</param>
        /// <param name="channelSelectedIndex">通道编号</param>
        /// <param name="startDateTime">时间</param>
        /// <returns>01_12_20140812114530.avi视频文件名</returns>
        public static string GetVideoFileName(int nvrSelectedIndex, int channelSelectedIndex, 
            DateTime startDateTime)
        {
            string nvrSelectedIndexString = (nvrSelectedIndex > 10) ? nvrSelectedIndex.ToString() :
                "0" + nvrSelectedIndex.ToString();
            string channelSelectedIndexString = (channelSelectedIndex > 10) ? channelSelectedIndex.ToString() :
                "0" + channelSelectedIndex.ToString();
            return nvrSelectedIndexString + "_" + channelSelectedIndexString + "_"
                + startDateTime.ToString("yyyyMMddHHmmss") + ".avi";
        }

        /// <summary>
        /// 在文件夹下根据时间条件搜索文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>文件列表</returns>
        public static List<SearchVideoFileInfo> SearchVideoFileByTime(string filePath,
            DateTime startTime, DateTime endTime)
        {

            List<SearchVideoFileInfo> videoFileInfoList = new List<SearchVideoFileInfo>();
            DirectoryInfo saveDir = new DirectoryInfo(filePath);
            FileInfo[] dirFiles = saveDir.GetFiles(); //获取当前目录下的文件列表

            for (int i = 0; i < dirFiles.Length; i++)
            {
                try
                {
                        FileInfo everyFile = dirFiles[i];
                        string everyFileName = everyFile.Name;
                        string everyFileFullName = everyFile.FullName;
                        string everyFileLength = everyFile.Length + "字节";
                        DateTime everyFileCreateTime = everyFile.CreationTime;
                        DateTime everyFileLastWriteTime = everyFile.LastWriteTime;
                        if (everyFileCreateTime > startTime &&  everyFileCreateTime < endTime)
                        {
                            videoFileInfoList.Add(new SearchVideoFileInfo(everyFileName, everyFileLength,
                                everyFileCreateTime, everyFileLastWriteTime));
                        }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(LogType.Error, "获取文件信息失败");
                    continue;
                }
            }
            return videoFileInfoList;
        }


        public List<List<SearchVideoFileInfo>> SplitSearchFileList(List<SearchVideoFileInfo>
            bigFileList, int groupCount)
        {
            List<List<SearchVideoFileInfo>> searchFileListGroup = new
                List<List<SearchVideoFileInfo>>();

            for (int i = 0; i < bigFileList.Count; )
            {
                List<SearchVideoFileInfo> cellList = new List<SearchVideoFileInfo>();
                cellList = bigFileList.Take(groupCount).Skip(i).ToList();
            }

            return searchFileListGroup;
        }

        public static string GetDateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
        }

        public static int Min2Subtle(int min)
        {
            return min * 60 * 1000;
        }

        public static long GetFileLength(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }


    }
}

    
