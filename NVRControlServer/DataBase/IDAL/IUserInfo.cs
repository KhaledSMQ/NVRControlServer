using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.DataBase.Modle;

namespace NVRControlServer.DataBase.IDAL
{

    public interface IUserInfo
    {

        bool Insert(UserInfoData data);
        bool DeleteByPK(int id);
        bool DeleteByPK(int[] id);
        bool UpdateByPK(UserInfoData data);

        UserInfoData GetDetailByPK(int id);

    }
}
