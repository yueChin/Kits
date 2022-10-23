using System;
using Kits.DevlpKit.Helpers.StringHelpers;

namespace Kits.ClientKit.Handlers
{
    public static class TimeHandler
    {
        private static readonly DateTime s_dateTime1970 = new System.DateTime(1970, 1, 1, 0, 0, 0);
        private static readonly DateTime s_dateTime1969 = new System.DateTime(1969, 12, 31, 19, 0, 0);

        public static DateTime NowTime => DateTime.Now;

        public static uint GetUtcTimeStamp(DateTime dateTime)
        {
            return (uint)(dateTime - s_dateTime1970).TotalSeconds;
        }

        public static string Format(string format, int one, int two, int three)
        {
            return StringHelper.Format(format, one.ToString("D2"), two.ToString("D2"), three.ToString("D2"));
        }

        public static string Format(string format, int one, int two)
        {
            return StringHelper.Format(format, one.ToString("D2"), two.ToString("D2"));
        }

        public static string TimeStampToString(uint timeStamp)
        {
            DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            //DateTime dtUTC = dto.DateTime;
            DateTime dtLocal = dto.LocalDateTime;

            //判断显示格式
            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);

            uint currentUnixSecond = (uint)DateTimeOffset.Now.ToUnixTimeSeconds();
            uint timeInterval = currentUnixSecond - timeStamp;

            uint todaySecond = (uint)(now - today).TotalSeconds;
            if (timeInterval < todaySecond)
            {
                //显示今天
                return dtLocal.ToString("HH:mm");
            }

            DateTime yesterday = today.AddDays(-1);
            uint yesterdaySecond = (uint)(now - yesterday).TotalSeconds;
            if (timeInterval > todaySecond && timeInterval < yesterdaySecond)
            {
                //显示昨天
                return dtLocal.ToString("昨天 HH:mm");
            }

            DateTime weekday = today.AddDays(-7);
            uint weekdaySecond = (uint)(now - weekday).TotalSeconds;
            if (timeInterval < weekdaySecond)
            {
                //显示一星期内
                //return dtLocal.ToString(StringHelper.Format("{0} HH:mm", GetLocalizationWeek(dtLocal.DayOfWeek)));
            }

            //显示具体日期
            return dtLocal.ToString("yyyy-MM-dd HH:mm");
        }

        // public static string GetLocalizationWeek(DayOfWeek dayOfWeek)
        // {
        //     if (!GameComponents.Localization) 
        //         return dayOfWeek.ToString();
        //     switch (dayOfWeek)
        //     {
        //         case DayOfWeek.Monday:
        //             return GameComponents.Localization.GetText("星期一");
        //         case DayOfWeek.Tuesday:
        //             return GameComponents.Localization.GetText("星期二");
        //         case DayOfWeek.Wednesday:
        //             return GameComponents.Localization.GetText("星期三");
        //         case DayOfWeek.Thursday:
        //             return GameComponents.Localization.GetText("星期四");
        //         case DayOfWeek.Friday:
        //             return GameComponents.Localization.GetText("星期五");
        //         case DayOfWeek.Saturday:
        //             return GameComponents.Localization.GetText("星期六");
        //         case DayOfWeek.Sunday:
        //             return GameComponents.Localization.GetText("星期六");
        //     }
        //
        //     return string.Empty;
        // }

        public static string TimeToTwoUnitString(long seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            if (t.TotalDays > 1)
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}天{1}小时)", t.Days, t.Hours));
            }
            else if (t.TotalHours > 1)
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}小时{1}分钟"), t.Hours, t.Minutes);
            }
            else
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}分钟"), (int)t.TotalMinutes);
            }

            return null;
        }

        /// <summary>
        /// 格式化为 10:48（小时:分钟） 格式
        /// </summary>
        /// <param name="seconds"></param>
        public static string TimeToSymbolHourString(long seconds)
        {
            if (seconds < 0) return seconds.ToString();
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            if (t.TotalHours > 1)
            {
                //大于一小时
                return StringHelper.Format("{0}:{1}", (t.Hours + t.Days * 24).ToString("D2"), t.Minutes.ToString("D2"));
            }

            //小于1小时，显示分钟
            return StringHelper.Format("00:{0}", t.Minutes.ToString("D2"));
        }

        /// <summary>
        /// 格式化为 48:50（分钟:秒数） 格式
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string TimeToSymbolMinusString(long seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            return StringHelper.Format("{0}:{1}", t.Minutes.ToString("D2"), t.Seconds.ToString("D2"));
        }

        public static string TimeToOneUnitString(long seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            if (t.TotalDays > 1)
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}天", (int)t.TotalDays));
            }
            else if (t.TotalHours > 1)
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}小时"), (int)t.TotalHours);
            }
            else
            {
                //return StringHelper.Format(GameComponents.Localization.GetText("{0}分钟"), (int)t.TotalMinutes);
            }

            return null;
        }

        public static long CurrentTimeOfSecond()
        {
            System.DateTime startTime = s_dateTime1970; // 当地时区
            long timeStamp = (long)(NowTime - startTime).TotalSeconds; // 相差秒数
            return timeStamp;
        }

        public static int GetTimeIntervalType(long time, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(time, isMs);
            if (sp.Days > 0)
                return 0;
            else if (sp.Hours > 0)
                return 1;
            else if (sp.Minutes >= 30)
                return 2;
            else if (sp.Minutes >= 10)
                return 3;
            else if (sp.Minutes >= 5)
                return 4;
            else
                return 5;
        }

        public static string GetTimeInterval(long time, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(time, isMs);
            if (sp.Days > 0)
                return sp.Days + "天前";
            else if (sp.Hours > 0)
                return sp.Hours + "小时前";
            else if (sp.Minutes >= 30)
                return "半小时前";
            else if (sp.Minutes >= 10)
                return "10分钟前";
            else if (sp.Minutes >= 5)
                return "5分钟前";
            else
                return "刚刚";
        }

        public static string GetTimeStampValid(long timeStamp, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(timeStamp, isMs, true);
            if (sp.Days > 0)
                return sp.Days + "天";
            else if (sp.Hours > 0)
                return sp.Hours + "小时";
            else if (sp.Minutes > 0)
                return sp.Minutes + "分钟";
            else if (sp.Seconds > 0)
                return sp.Seconds + "秒";
            else
                return "已过期";
        }

        /// <summary>
        /// 获取时间是否有效（是否过期）
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="is5Clock"></param>
        /// <returns></returns>
        public static bool GetIsTimeValid(long timeStamp, bool is5Clock = false)
        {
            TimeSpan sp = GetTimeSpan(timeStamp, is5Clock, true);
            bool isValid = false;
            isValid = sp.Days > 0 || sp.Hours > 0 || sp.Minutes > 0 || sp.Seconds > 0;
            return isValid;
        }

        /// <summary>
        /// 0 今天5点前 1 今天 2 明天5点后
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="isMs"></param>
        /// <returns></returns>
        public static int GetTimeStampType(long timeStamp, bool isMs = false)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime datetime = isMs ? startTime.AddMilliseconds(timeStamp) : startTime.AddSeconds(timeStamp);
            DateTime nowTime = NowTime;
            DateTime todayFiveHour = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 5, 0, 0);
            bool dayTime = nowTime.Hour >= 5;
            TimeSpan sp = dayTime ? datetime - todayFiveHour : todayFiveHour - datetime;
            //TimeSpan spt = ServerData.GetInstance().NowDataTime - datetime ;
            //Debug.LogError($"{spt.Days }   {spt.Hours}  {spt.Minutes}  {spt.Seconds} ");
            if (sp.Days > 0)
            {
                return dayTime ? 2 : 0;
            }
            else if (sp.Hours > 0 || sp.Minutes > 0 || sp.Seconds > 0)
            {
                return 1;
            }
            else
            {
                return dayTime ? 0 : 2;
            }
        }

        public static int GetTime(long timeStamp, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(timeStamp, isMs);
            return sp.Minutes * 60 + sp.Seconds;
        }

        /// <summary>
        /// 获取剩余时间(建议与时间戳是否有效一起搭配使用)
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="isLocation"></param>
        /// <param name="is5Clock"></param>
        /// <param name="isFuture"></param>
        /// <param name="isMinHours"></param>
        /// <returns></returns>
        public static string GetLeftTime(long timeStamp, bool isLocation = false, bool is5Clock = false, bool isFuture = false, bool isMinHours = false)
        {
            //#if UNITY_EDITOR
            TimeSpan sp = GetTimeSpan(timeStamp, is5Clock, false, isFuture);
            bool isValid = sp.Days > 0 || sp.Hours > 0 || sp.Minutes > 0 || sp.Seconds > 0;
            if (isValid)
            {
                int Second = sp.Seconds;
                int Minute = sp.Minutes;
                int Hour = sp.Hours;
                int Day = sp.Days;

                string Hours = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string Minutes = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string Seconds = Second < 10 ? $"0{Second}" : $"{Second}";
                if (isMinHours)
                {
                    if (Day > 0)
                    {
                        return isLocation ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                    }
                    else
                    {
                        if (Hour > 0)
                        {
                            return isLocation ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                        }
                        else
                        {
                            return isLocation ? $"1小时" : $"One Hour";
                        }
                    }
                }
                else
                {
                    if (Day > 0)
                    {
                        return isLocation ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                    }
                    else if (Hour > 0)
                    {
                        return isLocation ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                    }
                    else if (Minute > 0)
                    {
                        return isLocation ? $"{Minutes}分钟 {Seconds}秒" : $"{Minutes}: {Seconds}";
                    }
                    else
                    {
                        return isLocation ? $"{Seconds}秒" : $"{Seconds}";
                    }
                }

            }
            else
            {
                return "已结束";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secondsDuration">时间间隔</param>
        /// <param name="isLocation"></param>
        /// <param name="type">0 全  1 获取最大  2 获取天/小时  3 小时:分:秒 4 最大两位</param>
        /// <returns></returns>
        public static string GetFormatTime(int secondsDuration, bool isLocation = true, int type = 0)
        {
            int Second = secondsDuration % 60;
            int Minute = secondsDuration / 60;
            int Hour = Minute / 60;
            int Day = Hour / 24;
            if (Minute >= 60)
                Minute = Minute % 60;
            if (Hour >= 24 && type != 3)
                Hour = Hour % 24;

            if (type == 1)
            {
                if (Day > 0)
                {
                    return isLocation ? $"{Day}天" : $"{Day}";
                }
                else if (Hour > 0)
                {
                    return isLocation ? $"{Hour}小时" : $"{Hour}";
                }
                else if (Minute > 0)
                {
                    return isLocation ? $"{Minute}分钟" : $"{Minute}";
                }
                else
                {
                    return isLocation ? $"{Second}秒" : $"{Second}";
                }
            }
            else if (type == 2)
            {
                Hour = Hour == 0 ? (Minute > 0 || Second > 0) ? 1 : 0 : Hour;
                return isLocation ? $"{Day}天{Hour}小时" : $"{Day}:{Hour}";
            }
            else if (type == 3)
            {
                string Hours = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string Minutes = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string Seconds = Second < 10 ? $"0{Second}" : $"{Second}";
                return isLocation ? $"{Hours}小时 {Minutes}分 {Seconds}秒" : $"{Hours}: {Minutes}: {Seconds}";
            }
            else if (type == 4)
            {
                string Hours = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string Minutes = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string Seconds = Second < 10 ? $"0{Second}" : $"{Second}";
                if (Day > 0)
                {
                    return isLocation ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                }
                else if (Hour > 0)
                {
                    return isLocation ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                }
                else if (Minute > 0)
                {
                    return isLocation ? $"{Minutes}分钟 {Seconds}秒" : $"{Minutes}: {Seconds}";
                }
                else
                {
                    return isLocation ? $"{Seconds}秒" : $"{Seconds}";
                }
            }
            else
            {
                string day = Day.ToString();
                string hour = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string minute = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string second = Second < 10 ? $"0{Second}" : $"{Second}";
                return isLocation ? Day == 0 ? $"{hour}时{minute}分{second}秒" : $"{day}天{hour}时{minute}分{second}秒" : Day == 0 ? $"{hour}:{minute}:{second}" : $"{day}:{hour}:{minute}:{second}";
            }
        }

        public static TimeSpan GetTimeSpan(long timeStamp, bool is5Clock = false, bool isMs = false, bool isFuture = false)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(is5Clock ? s_dateTime1969 : s_dateTime1970);
            DateTime datetime = isMs ? startTime.AddMilliseconds(timeStamp) : startTime.AddSeconds(timeStamp);

            return isFuture ? datetime - NowTime : NowTime - datetime;
        }
    }
}