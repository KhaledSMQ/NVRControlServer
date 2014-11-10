using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NVRControlServer.Utils.Log
{
    /// <summary>
    /// 日志信息类型
    /// </summary>
    public enum LogType
    {
        Trace,
        Error,
        SQL,
        SQLError,
        SendErro,
        ReceiveErro
    }

    public class LogManager
    {

        #region 1.变量属性

        #region 1.1 变量
        private string logFileName = string.Empty;
        //private string logPath = "Log";
        private string logPath = System.Windows.Forms.Application.StartupPath + "\\Log.txt";
        private string logFileExtName = "log";
        private bool writeLogTime = true;
        private bool logFileNameEndWithDate = true;
        private Encoding logFileEncoding = Encoding.UTF8;
        private object obj = new object();
        #endregion 1.1 变量

        #region 1.2 属性
        /// <summary>
        /// 文件路径
        /// </summary>
        public string LogPath
        {
            get
            {
                if (this.logPath == null || this.logPath == string.Empty)
                {
                    this.logPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return this.logPath;
            }
            set
            {
                this.logPath = value;
                if (this.logPath == null || this.logPath == string.Empty)
                {
                    this.logPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                {
                    try
                    {
                        if (this.logPath.IndexOf(Path.VolumeSeparatorChar) >= 0)
                        {
                            /*绝对路径*/
                        }
                        else
                        {
                            this.logPath = AppDomain.CurrentDomain.BaseDirectory + this.logPath;
                        }
                    }
                    catch
                    {
                        this.logPath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                    if (!this.logPath.EndsWith(@"\"))
                        this.logPath += @"\";
                }
            }
        }

        /// <summary>
        /// Log文件扩展名
        /// </summary>
        public string LogFileExtName
        {
            get { return this.logFileExtName; }
            set { this.logFileExtName = value; }
        }

        /// <summary>
        /// 是否在每个Log行前面添加当前时间
        /// </summary>
        public bool WriteLogTime
        {
            get { return this.writeLogTime; }
            set { this.writeLogTime = value; }
        }

        /// <summary>
        /// 日志文件是否带日期
        /// </summary>
        public bool LogFileNameWithDate
        {
            get { return logFileNameEndWithDate; }
            set { logFileNameEndWithDate = value; }
        }

        /// <summary>
        /// 日志文件字符编码
        /// </summary>
        public Encoding LogFileEncoding
        {
            get { return logFileEncoding; }
            set { logFileEncoding = value; }
        }
        #endregion 1.2 属性

        #endregion 1.变量属性

        #region 2.构造方法

        #region 2.1 无参构造方法
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public LogManager()
        {
            this.logPath = "Log";
            this.logFileExtName = "log";
            this.writeLogTime = true;
            this.logFileNameEndWithDate = true;
            this.logFileEncoding = Encoding.UTF8;
        }
        #endregion 2.1 无参构造方法

        #region 2.2 有参构造方法
        /// <summary>
        /// 有参构造方法
        /// </summary>
        /// <param name="logPath">文件路径</param>
        /// <param name="logFileExtName">文件拓展名</param>
        /// <param name="writeLogTime">是否将时间写入文件</param>
        public LogManager(string logPath, string logFileExtName, bool writeLogTime)
        {
            this.logPath = logPath;
            this.logFileExtName = logFileExtName;
            this.logFileNameEndWithDate = true;
            this.logFileEncoding = Encoding.UTF8;
        }
        #endregion 2.2有参构造方法

        #endregion 2.构造方法

        #region 3.公有方法

        #region 3.1 日志信息写进文件
        /// <summary>
        /// 将日志信息写进文件
        /// </summary>
        /// <param name="logFile">日志文件</param>
        /// <param name="msg">信息</param>
        public void WriteLog(string logFile, string msg)
        {
            lock (obj)
            {
                try
                {
                    string dateString = string.Empty;
                    if(this.logFileNameEndWithDate || logFile.Length == 0)
                    {
                        dateString  = DateTime.Now.ToString("yyyyMMdd");
                    }

                    logFileName = string.Format("{0}, {1}, {2}, {3}",
                                                                        this.logPath,
                                                                        logFile,
                                                                        dateString,
                                                                        this.logFileExtName);

                    using (StreamWriter sw = new StreamWriter(logFileName, true, logFileEncoding))
                    {
                        if(writeLogTime)
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:") + msg);
                        }
                        else
                        {
                            sw.WriteLine(msg);
                        }
                    }
                }
                catch
                {
                }
           }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFile"> 日志信息类型</param>
        /// <param name="msg">信息内容</param>
        public void WriteLog(LogType logFile, string msg)
        {
            this.WriteLog(logFile.ToString(), msg);
        }

        public void WriteLog(string msg)
        {
            this.WriteLog(string.Empty, msg);
        }
        #endregion 3.1 日志信息写进文件

        #endregion 3.公有方法

    }
}
