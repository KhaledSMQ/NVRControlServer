#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：            2014.7.14
/// 
/// 功能：                                    时间操作 
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NVRControlServer.Storage.Utils
{
    public static class TimeHelper
    {
        /// <summary>
        /// 随机字符
        /// </summary>
        private static char[] constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',  
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };


        /// <summary>
        /// 用于生成随机字符串
        /// </summary>
        /// <param name="Length">随机字符串长度</param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        ///  将星期几转换成整型数值
        /// </summary>
        /// <param name="mDateOfWeekStr">输入星期几英文字符串</param>
        /// <returns> success:1,2,3,4,5,6,7  failed:-1</returns>
        public static int DateOfWeekString2Int(string mDateOfWeekStr)
        {
            int  mDateInt = 0;

            switch (mDateOfWeekStr)
            {
                case "Monday":
                    mDateInt = 1;
                    break;
                case "Tuesday":
                    mDateInt = 2;
                    break;
                case "Wednesday":
                    mDateInt = 3;
                    break;
                case "Thursday":
                    mDateInt = 4;
                    break;
                case "Friday":
                    mDateInt = 5;
                    break;
                case "Saturday":
                    mDateInt = 6;
                    break;
                case "Sunday":
                    mDateInt = 7;
                    break;
                default:
                    mDateInt = -1;
                    break;
            }
            return mDateInt;
        }

        public static string ChDateWeekTime2EnDateWeekTime(string mChinDateStr)
        {
            string EnDateWeekStr;

            switch (mChinDateStr)
            {
                case "一":
                    EnDateWeekStr = "Monday";
                    break;
                case "二":
                    EnDateWeekStr = "Tuesday";
                    break;
                case "三":
                    EnDateWeekStr = "Wednesday";
                    break;
                case "四":
                    EnDateWeekStr = "Thursday";
                    break;
                case "五":
                    EnDateWeekStr = "Friday";
                    break;
                case "六":
                    EnDateWeekStr = "Saturday";
                    break;
                case "日":
                    EnDateWeekStr = "Sunday";
                    break;
                default :
                    EnDateWeekStr = "Unkonwn";
                    break;
            }

            return EnDateWeekStr;
        }


        //计算计划中的星期几与当天星期的天数
        public static int GetDifferenceDay(string mNowDay, string mScheduleDay)
        {
            int mNowDayInt = DateOfWeekString2Int(mNowDay);
            int mSheduleDayInt = DateOfWeekString2Int(mScheduleDay);

            if (mSheduleDayInt - mNowDayInt >= 0)
                return (mSheduleDayInt - mNowDayInt);
            else
                return (mSheduleDayInt + 7 - mNowDayInt);
        }


        public static DateTime StringToDateTime(string timeString)
        {
            return DateTime.ParseExact(timeString, 
                "yyyyMMddHHmmss", 
                System.Globalization.CultureInfo.CurrentCulture);
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }


        // 判断星期几
        public static string GetCNDateOfWeek()
        {
            string str = DateTime.Now.DayOfWeek.ToString();
            str.IndexOf("1");
            string strWeek = "";
            switch (str)
            {
                case "Monday":
                    strWeek = "星期一";
                    break;
                case "Tuesday":
                    strWeek = "星期二";
                    break;
                case "Wednesday":
                    strWeek = "星期三";
                    break;
                case "Thursday":
                    strWeek = "星期四";
                    break;
                case "Friday":
                    strWeek = "星期五";
                    break;
                case "Saturday":
                    strWeek = "星期六";
                    break;
                case "Sunday":
                    strWeek = "星期日";
                    break;
            }
            return strWeek;
        }

    }
}
