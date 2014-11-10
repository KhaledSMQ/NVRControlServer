using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Utils.Log
{
    public static class Log
    {
        private static LogManager logManager;

        static Log()
        {
            logManager = new LogManager();
        }

        public static void WriteLog(LogType logFile, string msg)
        {
            logManager.WriteLog(logFile, msg);
        }

        public static void WriteLog(string logFile, string msg)
        {
            logManager.WriteLog(logFile, msg);
        }

    }
}
