using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.DataBase.Modle;

namespace NVRControlServer.DataBase.IDAL
{
    public interface IVideoInfo
    {
        bool Insert(VideoInfoData data);
        IList<VideoInfoData> GetVideoByTime(VideoInfoData cond);
        IList<VideoInfoData> GetSingleVideoByCond(VideoInfoData cond);
        bool UpdateVideoTagByName(VideoInfoData cond);

    }
}
