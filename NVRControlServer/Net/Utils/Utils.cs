﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


using NVRControlServer.Net.Model;
using System.Reflection;
using System.Security.Cryptography;

namespace NVRControlServer.Net.Utils
{

    class Transform
    {
        
        public static byte[] MinInt2Bytes(int k)
        {
            byte[] b = new byte[1];
            b[0] = (byte)k;
            return b;
        }

        public static int Byte2MinInt(byte[] b)
        {
            int k = (int)b[0];
            return k;
        }

        public static int Byte2MinInt(byte b)
        {
            int k = (int)b;
            return k;
        }

        public static byte[] Int2Bytes(int k)
        {
            byte[] b = new byte[2];
            b[0] = (byte)(k >> 7);
            b[1] = (byte)(k & 127);
            return b;
        }

        public static int Bytes2Int(byte[] b)
        {
            int k = b[0];
            k <<= 7;
            k += b[1];
            return k;
        }

        public static byte[] Long2Bytes(long n)
        {
            return BitConverter.GetBytes(n);
        }

        public static long Bytes2Long(byte[] bytes)
        {
            return BitConverter.ToInt64(bytes, 0); 
        }

        public static int parseByte(byte[] b)
        {
            int k = b[0];
            k <<= 7;
            k += b[1];
            return k;
        }

        public static long byte2Long(byte[] data, int length)
        {
            long size = 0;

            for (int i = 0; i < length; i++)
            {
                int val = (int)(data[i] - '0');
                size = size * 10 + val;
            }
            return size;
        }

        public static byte[] String2Byte(string temp)
        {
            byte[] buffer = System.Text.Encoding.Default.GetBytes(temp);
            return buffer;
        }

        public static string Byte2String(byte[] temp)
        {
            string buffer = System.Text.Encoding.Default.GetString(temp);
            return buffer;
        }

        public static int String2Int(string temp)
        {
            int val = int.Parse(temp);
            return val;
        }
        public static string[] getStrings(string temp)
        {
            string[] splitString = temp.Split(',');
            return splitString;
        }

        public static string[] GetStrings(string temp, char ch)
        {
            string[] splitString = temp.Split(ch);
            return splitString;
        }

        public static char[] stringParseCharArray(string temp)
        {
            char[] buff = new char[temp.Length];
            buff = temp.ToCharArray();
            return buff;
        }

        public static string charArrayParseString(char[] temp)
        {
            string str = new string(temp, 0, temp.Length);
            return str;
        }


        public static DialogResult Show(string op)
        {
            return MessageBox.Show(op, "警告!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        //将一个序列化对象，返回一个byte[]
        public static byte[] Serialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Dispose();
            return data;
        }

        //将一个序列化的byte[]还原成对象
        public static object Deserialiaze(byte[] objbytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Binder = new UBinder();
            MemoryStream stream = new MemoryStream(objbytes);
            object obj = bf.Deserialize(stream);
            stream.Dispose();
            return obj;
        }

        public class UBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                if (typeName.EndsWith("DKClient.PlayBackModule.Net.Model.ResponeTraFransfersFile"))
                    return typeof(NVRControlServer.Net.Model.ResponeTraFransfersFile);
                if (typeName.EndsWith("DKClient.PlayBackModule.Net.Vo.VideoInfoDataVo"))
                    return typeof(NVRControlServer.Net.Vo.VideoInfoDataVo);
                Assembly ass = Assembly.GetExecutingAssembly();
                return ass.GetType(typeName);
            }
        } 

        public static DateTime Bytes2toDateTime(byte[] tem)
        {

            return DateTime.FromBinary(BitConverter.ToInt64(tem, 0));
        }

        public static byte[] DateTime2Bytes(DateTime time)
        {
            string timeStr = DateTime2String(time);
            return System.Text.Encoding.Default.GetBytes(timeStr);
        }

        public static DateTime String2DateTime(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
        }

        public static string DateTime2String(DateTime dateTime)
        {
             return dateTime.ToString("yyyyMMddHHmmss",System.Globalization.CultureInfo.CurrentCulture);
        }
    }

    public static class MD5Helper
    {
        private static string ByteArrayToHexString(byte[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.AppendFormat("{0:X2}", value);
            }
            return sb.ToString();
        }


        public static string CreateMD5(string fileName)
        {
            string hashStr = string.Empty;
            try
            {
                FileStream fs = new FileStream(
                    fileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hash = md5.ComputeHash(fs);
                hashStr = ByteArrayToHexString(hash);
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hashStr;
        }
    }


    public class BufferHelper
    {
        public static byte[] Serialize(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bf.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Dispose();
            return data;
        }

        public static object Deserialize(byte[] data, int index)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data, index, data.Length - index);
            object obj = bf.Deserialize(stream);
            stream.Dispose();
            return obj;
        }

    }


}
