using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NVRControlServer.Storage.Utils
{
    public class DriveInfoModel
    {
        /// <summary>   
        /// 磁盘名称   
        /// </summary>   
        public string DriveName { get; set; }
        /// <summary>   
        /// 磁盘总容量   
        /// </summary>   
        public string DriveSize { get; set; }
        /// <summary>   
        /// 磁盘剩余空间   
        /// </summary>   
        public string DriveFree { get; set; }
        /// <summary>   
        /// 磁盘已使用空间   
        /// </summary>   
        public string DriveSpent { get; set; }
        /// <summary>   
        /// 磁盘格式FAT,FAT32,NTFS   
        /// </summary>   
        public string DriveFormat { get; set; }

        public DriveInfoModel()
        {
        }

     }

    public class IOInfo
    {

        /// <summary>   
        /// 获取所有磁盘驱动器信息   
        /// </summary>   
        /// <returns></returns> 
        public static IList<DriveInfoModel> GetAllDriver()
        {
            IList<DriveInfoModel> models = new List<DriveInfoModel>();
            DriveInfo[] dis = DriveInfo.GetDrives();

            foreach(DriveInfo di in dis)
            {
                if (di.DriveType == DriveType.Fixed && di.IsReady)
                {
                    DriveInfoModel model = new DriveInfoModel();
                    model.DriveFormat = di.DriveFormat;
                    model.DriveName = di.Name;
                    model.DriveSize = (di.TotalSize / 1024 / 1024).ToString();
                    model.DriveFree = (di.TotalFreeSpace / 1024 / 1024).ToString();
                    model.DriveSpent = ((di.TotalSize - di.TotalFreeSpace) / 1024 / 1024).ToString();
                    models.Add(model);
                }
            }

            return models;
        }

        public static DriveInfoModel GetDrive(string DriveName)
        {
            DriveInfo[] dis = DriveInfo.GetDrives();
            foreach (DriveInfo di in dis)
            {
                if (di.DriveType == DriveType.Fixed && di.IsReady && di.Name.Substring(0, 1).ToUpper() == DriveName.ToUpper())
                {
                    DriveInfoModel model = new DriveInfoModel();
                    model.DriveFormat = di.DriveFormat;                                     //磁盘格式   
                    model.DriveName = di.Name;                                              //磁盘名称   
                    model.DriveSize = (di.TotalSize / 1024 / 1024).ToString();         //磁盘大小计算   
                    model.DriveFree = (di.TotalFreeSpace / 1024 / 1024).ToString();
                    model.DriveSpent = ((di.TotalSize - di.TotalFreeSpace) / 1024 / 1024).ToString();
                    return model;
                }
            }
            return null;
        }

        /// <summary>   
        /// 获取指定文件夹的大小   
        /// </summary>   
        /// <param name="path">目录路径 C://VideoStream//</param>   
        /// <returns>Null则表示文件夹不存在</returns>   
        public static string GetDirectorySize(string path)
        {
            //取得目录大小   
            long l = getDirectorySize(path);
            if (l == -1)
                //目录无效   
                return null;
            else
            {
                //根据大小返回不同单位   
                if ((l / 1024) < 1024)
                {
                    return (l / 1024).ToString() + "KB";
                }
                else
                {
                    return (l / 1024 / 1024).ToString() + "MB";
                }
            }

        }

        /// <summary>   
        /// 获取指定文件夹的大小   
        /// </summary>   
        /// <param name="path"></param>   
        /// <returns></returns>   
        private static long getDirectorySize(string path)
        {
            long result = 0;
            DirectoryInfo info = new DirectoryInfo(path);
            //目录无效   
            if (!info.Exists) return -1;
            foreach (FileInfo fi in info.GetFiles())
            {
                //累加当前文件的大小   
                result += fi.Length;
            }
            foreach (DirectoryInfo di in info.GetDirectories())
            {
                //累加当前文件夹大小，递归   
                result += getDirectorySize(di.FullName);
            }
            return result;
        }

        /// <summary>   
        /// 删除指定的文件夹   
        /// </summary>   
        /// <param name="path"></param>   
        /// <returns></returns>   
        public static bool DeleteDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            //目录无效   
            if (!info.Exists) return false;
            bool result = false;
            try
            {
                info.Delete(true);//删除动作   
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>   
        /// 清除指定文件夹里面的文件，文件夹   
        /// </summary>   
        /// <param name="path"></param>   
        /// <returns></returns>   
        public static bool ClearDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            //目录无效   
            if (!info.Exists) return false;
            bool result = false;
            try
            {

                foreach (DirectoryInfo di in info.GetDirectories())
                {//删除指定文件夹下面的所有文件夹   
                    di.Delete(true);
                }
                foreach (FileInfo fi in info.GetFiles())
                {//删除指定文件夹下面的所有文件   
                    fi.Delete();
                }
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }  
    }




    class DriverInfoLibrary
    {
        

    }
}
