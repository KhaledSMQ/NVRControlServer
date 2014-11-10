using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using NVRControlServer.DataBase.Module;
using NVRControlServer.DataBase.Dao;

namespace NVRControlServer.DataBase.Service
{
    public class VideoDataService
    {
        private DataBaseDao serviceDao;
        private SqlConnection serviceConn;
        private SqlCommand serviceCommand;
        private List<VideoInfo> videoInfoList;
        private string serviceSqlErr;

        public List<VideoInfo> VideoInfoList
        {
            get { return videoInfoList; }
            set { videoInfoList = value; }
        }


        public VideoDataService()
        {
            serviceDao = new DataBaseDao();
        }

        public Boolean VideoDataInsert(VideoInfo videoInfo)
        {

            using (serviceConn = serviceDao.CreateConnection())
            {
                serviceCommand = new SqlCommand();
                try
                {

                    string videoDataInsertString = "insert into VideoInfo(nvrId, nvrName, channelId, channelName, startTime,endTime, fileName, filePath)";
                    videoDataInsertString += "values('"  + videoInfo.NvrId + "','"
                                        + videoInfo.NvrName + "','" + videoInfo.ChannelId + "','" +
                                        videoInfo.ChannelName + "','" + videoInfo.StartTime + "','" +
                                        videoInfo.EndTime + "','" + videoInfo.VideoFileName + "','" +
                                        videoInfo.VideoFilePath + "')";

                    serviceCommand.CommandText = videoDataInsertString;
                    serviceCommand.Connection = serviceConn;
                    serviceConn.Open();

                    if (serviceCommand.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("insertOk!");
                        return true;
                    }
                    else
                        return false;

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                finally
                {
                    serviceCommand.Connection.Close();
                    serviceConn.Close();
                }
            }
        }


        public List<VideoInfo> GetTimeVideoFiles(int nvrId, int channelId, DateTime startTime, DateTime endTime)
        {
            List<VideoInfo> videoInfoList = new List<VideoInfo>();
            string videoDataSelectString = string.Format("select * from VideoInfo where nvrId = '{0}' and " +
                "channelId = '{1}'  and startTime  > '{2}' and endTime <  '{3}'",nvrId, channelId, startTime, endTime);

            using (serviceConn = serviceDao.CreateConnection())
            {
                serviceCommand = new SqlCommand();
                try
                {
                    serviceCommand.CommandText = videoDataSelectString;
                    serviceCommand.Connection = serviceConn;
                    serviceConn.Open();
                    SqlDataReader serviceSqlreader = serviceCommand.ExecuteReader();

                    while (serviceSqlreader.Read())
                    {
                        VideoInfo videoInfo = new VideoInfo((int)serviceSqlreader[1], (string)serviceSqlreader[2],
                            (int)serviceSqlreader[3], (string)serviceSqlreader[4], (DateTime)serviceSqlreader[5],
                            (DateTime)serviceSqlreader[6], (string)serviceSqlreader[7], (string)serviceSqlreader[8]);
                        videoInfoList.Add(videoInfo);
                    }
                    return videoInfoList;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    serviceCommand.Connection.Close();
                    serviceConn.Close();
                }
            }
        }




    }
}
