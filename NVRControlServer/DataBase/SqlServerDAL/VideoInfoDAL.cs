using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.DataBase.IDAL;
using NVRControlServer.DataBase.Modle;
using EasyDBUtility;

namespace NVRControlServer.DataBase.SqlServerDAL
{
    public class VideoInfoDAL : IVideoInfo
    {
        private const string Insert_SQL = "insert into [VideoInfo] ([nvrId],[nvrName],[channelId],[channelName],[startTime],[endTime],[fileName],[filePath],[fileTag],[fileSize]) values (@nvrId,@nvrName,@channelId,@channelName,@startTime,@endTime,@fileName,@filePath,@fileTag,@fileSize)";
        //private const string Delete_By_ID_SQL = "delete from [VideoInfo] where id=@id";
        //private const string Delete_By_NvrChannelId_FileNam_SQL = "delete from [VideoInfo] where nvrId=@nvrId and channelId=@channelId and filePath=@filePath and fileName = @fileName";
        //private const string BatchDelete_By_ID_SQL = "delete from [VideoInfo] where id in({0})";
        //private const string Update_By_ID_SQL = "update [VideoInfo] set [nvrId]=@nvrId,[channelId]=@channelId,[startTime]=@startTime,[endTime]=@endTime,[fileName]=@fileName,[filePath]=@filePath,[fileTag]=@fileTag where id=@id";
        //private const string Select_By_ID_SQL = "select * from [VideoInfo] where id=@id";
        //private const string SelectAll_By_ID_SQL = "select * from [VideoInfo] ";
        private const string Select_By_NvrChannelId_SQL = "select * from [VideoInfo] where nvrId = @nvrId";
        private const string Select_By_Time_SQL = "select * from[VideoInfo]  where  nvrId = @nvrId and channelId = @channelId and startTime > @startTime and endTime < @endTime ";
        private const string Update_By_Name_SQL = "update [VideoInfo] set [fileTag]=@fileTag where [nvrId]=@nvrId and [channelId]=@channelId and [fileName]=@fileName";
        private const string Select_By_Cond = "select * from [VideoInfo] where nvrId=@nvrId and channelId = @channelId and [fileName] = @fileName";



        // 新增一条数据记录
        public bool Insert(VideoInfoData data)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Insert_SQL);
            helper.AddParameter(data);
            return helper.ExecuteNonQuery() > 0 ? true : false;
        }

        //通过时间条件查找
        public IList<VideoInfoData> GetVideoByTime(VideoInfoData cond)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Select_By_Time_SQL);
            helper.AddParameter(cond);
            List<VideoInfoData> list = helper.ExecuteReader<VideoInfoData>();
            return list;
        }

        //通过文件名来更新标签
        public bool UpdateVideoTagByName(VideoInfoData cond)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Update_By_Name_SQL);
            helper.AddParameter(cond);
            return helper.ExecuteNonQuery() > 0 ? true : false;
        }

        //通过条件查找
        public IList<VideoInfoData> GetSingleVideoByCond(VideoInfoData cond)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Select_By_Cond);
            helper.AddParameter(cond);
            List<VideoInfoData> list = helper.ExecuteReader<VideoInfoData>();
            return list;
        }


        ////根据主键值删除一条数据记录
        //public bool DeleteByPK(int id)
        //{
        //    SqlHelper helper = new SqlHelper();
        //    helper.CreateCommand(Delete_By_ID_SQL);
        //    helper.AddParameter("@id", id);
        //    return helper.ExecuteNonQuery() > 0 ? true : false;
        //}

        ////根据对象删除一条数据记录
        //public bool DeleteByData(VideoInfoData data)
        //{
        //    SqlHelper helper = new SqlHelper();
        //    helper.CreateCommand(Delete_By_NvrChannelId_FileNam_SQL);
        //    helper.AddParameter(data);
        //    return helper.ExecuteNonQuery() > 0 ? true : false;
        //}


        ////根据主键值批量删除数据记录
        //public bool DeleteByPK(int[] id)
        //{
        //    bool success = false;
        //    string strPKValue = string.Empty;
        //    foreach (int i in id)
        //    {
        //        strPKValue += i + ",";
        //    }
        //    SqlHelper helper = new SqlHelper();
        //    helper.BeginTransaction();
        //    try
        //    {
        //        helper.CreateCommand(string.Format(BatchDelete_By_ID_SQL, strPKValue.TrimEnd(',')));
        //        helper.ExecuteNonQuery();
        //        helper.Commit();
        //        success = true;
        //    }
        //    catch
        //    {
        //        helper.RollBack();
        //        success = false;
        //    }
        //    return success;
        //}

        ////根据主键值修改一条数据记录
        //public bool UpdateByPK(VideoInfoData data)
        //{
        //    SqlHelper helper = new SqlHelper();
        //    helper.CreateCommand(Update_By_ID_SQL);
        //    helper.AddParameter(data);
        //    return helper.ExecuteNonQuery() > 0 ? true : false;
        //}

        ////根据主键值获取一条数据记录详细信息
        //public VideoInfoData GetDetailByPK(int id)
        //{
        //    SqlHelper helper = new SqlHelper();
        //    helper.CreateCommand(Select_By_ID_SQL);
        //    helper.AddParameter("@id", id);
        //    return helper.ExecuteReaderSingle<VideoInfoData>();
        //}


    
    }
}
