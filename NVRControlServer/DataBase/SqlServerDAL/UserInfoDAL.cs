using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.DataBase.IDAL;
using NVRControlServer.DataBase.Modle;
using EasyDBUtility;

namespace NVRControlServer.DataBase.SqlServerDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoDAL : IUserInfo
    {
        private const string Insert_SQL = "insert into [UserInfo] ([userName],[userPassword],[uesrLevel]) values (@userName,@userPassword,@uesrLevel)";
        private const string Delete_By_ID_SQL = "delete from [UserInfo] where id=@id";
        private const string BatchDelete_By_ID_SQL = "delete from [UserInfo] where id in({0})";
        private const string Update_By_ID_SQL = "update [UserInfo] set [userName]=@userName,[userPassword]=@userPassword,[uesrLevel]=@uesrLevel where id=@id";
        private const string Select_By_ID_SQL = "select * from [UserInfo] where id=@id";
        private const string Select_By_NamePssword_SQL = "select * from [UserInfo] where userName=@userName and userPassword=@userPassword";
        private const string Select_By_Name_SQL = "select * from [UserInfo] where userName=@userName";
        private const string SelectAll_By_ID_SQL = "select * from [UserInfo] ";

        // 新增一条数据记录
        public bool Insert(UserInfoData data)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Insert_SQL);
            helper.AddParameter(data);
            return helper.ExecuteNonQuery() > 0 ? true : false;
        }

        // 根据主键值删除一条数据记录
        public bool DeleteByPK(int id)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Delete_By_ID_SQL);
            helper.AddParameter("@id", id);
            return helper.ExecuteNonQuery() > 0 ? true : false;
        }

        //根据主键值批量删除数据记录
        public bool DeleteByPK(int[] id)
        {
            bool success = false;
            string strPKValue = string.Empty;
            foreach (int i in id)
            {
                strPKValue += i + ",";
            }
            SqlHelper helper = new SqlHelper();
            helper.BeginTransaction();
            try
            {
                helper.CreateCommand(string.Format(BatchDelete_By_ID_SQL, strPKValue.TrimEnd(',')));
                helper.ExecuteNonQuery();
                helper.Commit();
                success = true;
            }
            catch
            {
                helper.RollBack();
                success = false;
            }
            return success;
        }

        // 根据主键值修改一条数据记录
        public bool UpdateByPK(UserInfoData data)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Update_By_ID_SQL);
            helper.AddParameter(data);
            return helper.ExecuteNonQuery() > 0 ? true : false;
        }

        //根据主键值获取一条数据记录详细信息
        public UserInfoData GetDetailByPK(int id)
        {
            SqlHelper helper = new SqlHelper();
            helper.CreateCommand(Select_By_ID_SQL);
            helper.AddParameter("@id", id);
            return helper.ExecuteReaderSingle<UserInfoData>();
        }





    }
}
