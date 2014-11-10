using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using NVRControlServer.DataBase.Model;
using NVRControlServer.DataBase.Dao;


namespace CS_Server.Service
{
    class ImageDataService
    {
        //public string imageDataInsert(ImageInfo img)
        //{

            //Console.WriteLine("into imagedataiinster");

            //string p_str_result = null;

            //DataBaseDao dao = new DataBaseDao();

            //try
            //{
            //    SqlConnection con = dao.createConnection();
            //    SqlCommand cmd = new SqlCommand();
            //    string img_str_insert = "insert into DataImage(Active, Deleted, CreateDate, CreateBy, UpdateDate, UpdateBy,Name, NodeID, Path, ImageDate)";
            //    img_str_insert += "values('" + img.getActive() + "','" + img.getDeleted() + "','"
            //                                + img.getCreateDate() + "','" + img.getCreateBy() + "','" + img.getUpdateDate() + "','"
            //                                + img.getUpdateBy() + "','" + img.getName() + "','" + img.getNodeId() + "','" + img.getPath() + "','" + img.getImageDate() + "')";

            //    cmd.CommandText = img_str_insert;
            //    cmd.Connection = con;
            //    con.Open();
            //    int p_int_inst = cmd.ExecuteNonQuery();
            //    if (p_int_inst == 1)
            //    {
            //        Console.WriteLine("insertOk!");
            //        p_str_result = "insertOK";
            //    }
            //    else
            //    {
            //        p_str_result = "insertNo";
            //    }
            //    cmd.Connection.Close();
            //    con.Close();
            //    return p_str_result;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    p_str_result = "addError";
            //    return p_str_result;
            //}

       // }
    }
}
