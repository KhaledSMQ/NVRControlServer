using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;


namespace NVRControlServer.DataBase.Dao
{
     public class DataBaseDao
     {
         #region 1.变量属性

         #region 1.1 变量
         private string dataBaseConnectionString;
         private SqlConnection dataBaseConnection;
        #endregion 1.1 变量

         #region 1.2 属性
         public string DataBaseConnectionString
         {
             get { return dataBaseConnectionString; }
             set { dataBaseConnectionString = value; }
         }

         public SqlConnection DataBaseConnection
         {
             get { return dataBaseConnection; }
             set { dataBaseConnection = value; }
         }
        #endregion 1.2 属性

         #endregion 1.变量属性

         #region 2. 构造方法

         #region 2.1 无参构造
         public DataBaseDao()
        {
            dataBaseConnectionString = 
                NVRControlServer.Properties.Settings.Default.DKServerConnectionString;
        }
         #endregion 2.1无参构造

         #endregion 2.构造方法



         /// <summary>
        /// 建立数据库连接
        /// </summary>
        /// <returns>返回sqlconnection对象</returns>
        public  SqlConnection CreateConnection()
        {
            return new SqlConnection(dataBaseConnectionString);
        }

        /// <summary>
        /// 执行sqlCommand
        /// </summary>
        /// <param name="sqlstr"></param>
        /// 
        public void getCom(string sqlstr)
        {
            SqlConnection sqlcon = CreateConnection();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcom.Dispose();
            sqlcon.Close();
        }

        /// <summary>
        /// 创建一个sqlDataReader对象
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns>返回sqlDataReader对象</returns>
        public SqlDataReader GetSqlReader(string sqlstr)
        {
            using (SqlConnection sqlCon = CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(sqlstr, sqlCon);
                sqlCon.Open();
                SqlDataReader sqlread = cmd.ExecuteReader();
                return sqlread;
            }
        }

    }
}
