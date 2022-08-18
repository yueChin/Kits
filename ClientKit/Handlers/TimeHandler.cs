using System;

namespace Kits.ClientKit.Handlers
{
    public static class TimeHandler
    {
        public static DateTime NowTime => DateTime.Now;

        public static long CurrentTimeofSecond()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(NowTime - startTime).TotalSeconds; // 相差秒数
            return timeStamp;
        }

        public static int GetTimeIntervalType(this long time, bool isMs = false)
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

        public static string GetTimeInterval(this long time, bool isMs = false)
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

        public static string GetTimeValid(this long time, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(time, isMs, true);
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
        /// <param name="time"></param>
        /// <param name="isMs"></param>
        /// <returns></returns>
        public static bool GetIsTimeValid(this long time, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(time, isMs, true);
            bool isValid = false;
            isValid = sp.Days > 0 || sp.Hours > 0 || sp.Minutes > 0 || sp.Seconds > 0;
            return isValid;
        }

        /// <summary>
        /// 0 今天5点前 1 今天 2 明天5点后
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isMs"></param>
        /// <returns></returns>
        public static int GetIsTimeType(this long time, bool isMs = false)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime datetime = isMs ? startTime.AddMilliseconds(time) : startTime.AddSeconds(time);
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

        public static int GetTime(this long time, bool isMs = false)
        {
            TimeSpan sp = GetTimeSpan(time, isMs);
            return sp.Minutes * 60 + sp.Seconds;
        }

        /// <summary>
        /// 获取剩余时间(建议与时间戳是否有效一起搭配使用)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="isChinese"></param>
        /// <returns></returns>
        public static string GetLeftTime(this long time, bool isChinese = true, bool is5Clock = false, bool isMinHours = false)
        {
            //#if UNITY_EDITOR
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(is5Clock ? new System.DateTime(1969, 12, 31, 19, 0, 0) : new System.DateTime(1970, 1, 1, 0, 0, 0));
            //#else
            //        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(is5Clock ?new System.DateTime(1970, 1, 1, 3, 0, 0) :new System.DateTime(1970, 1, 1, 8, 0, 0)); // 当地时区
            //#endif 
            DateTime datetime = startTime.AddSeconds(time);
            TimeSpan sp = datetime - NowTime;
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
                        return isChinese ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                    }
                    else
                    {
                        if (Hour > 0)
                        {
                            return isChinese ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                        }
                        else
                        {
                            return isChinese ? $"1小时" : $"One Hour";
                        }
                    }
                }
                else
                {
                    if (Day > 0)
                    {
                        return isChinese ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                    }
                    else if (Hour > 0)
                    {
                        return isChinese ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                    }
                    else if (Minute > 0)
                    {
                        return isChinese ? $"{Minutes}分钟 {Seconds}秒" : $"{Minutes}: {Seconds}";
                    }
                    else
                    {
                        return isChinese ? $"{Seconds}秒" : $"{Seconds}";
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
        /// <param name="seconds"></param>
        /// <param name="isChinese"></param>
        /// <param name="type">0 全  1 获取最大  2 获取天/小时  3 小时:分:秒 4 最大两位</param>
        /// <returns></returns>
        public static string GetFormatTime(this int seconds, bool isChinese = true, int type = 0)
        {
            int Second = seconds % 60;
            int Minute = seconds / 60;
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
                    return isChinese ? $"{Day}天" : $"{Day}";
                }
                else if (Hour > 0)
                {
                    return isChinese ? $"{Hour}小时" : $"{Hour}";
                }
                else if (Minute > 0)
                {
                    return isChinese ? $"{Minute}分钟" : $"{Minute}";
                }
                else
                {
                    return isChinese ? $"{Second}秒" : $"{Second}";
                }
            }
            else if (type == 2)
            {
                Hour = Hour == 0 ? (Minute > 0 || Second > 0) ? 1 : 0 : Hour;
                return isChinese ? $"{Day}天{Hour}小时" : $"{Day}:{Hour}";
            }
            else if (type == 3)
            {
                string Hours = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string Minutes = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string Seconds = Second < 10 ? $"0{Second}" : $"{Second}";
                return isChinese ? $"{Hours}小时 {Minutes}分 {Seconds}秒" : $"{Hours}: {Minutes}: {Seconds}";
            }
            else if (type == 4)
            {
                string Hours = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string Minutes = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string Seconds = Second < 10 ? $"0{Second}" : $"{Second}";
                if (Day > 0)
                {
                    return isChinese ? $"{Day}天 {Hours}小时" : $"{Day}: {Hours}";
                }
                else if (Hour > 0)
                {
                    return isChinese ? $"{Hours}小时 {Minutes}分钟" : $"{Hours}: {Minutes}";
                }
                else if (Minute > 0)
                {
                    return isChinese ? $"{Minutes}分钟 {Seconds}秒" : $"{Minutes}: {Seconds}";
                }
                else
                {
                    return isChinese ? $"{Seconds}秒" : $"{Seconds}";
                }
            }
            else
            {
                string day = Day.ToString();
                string hour = Hour < 10 ? $"0{Hour}" : $"{Hour}";
                string minute = Minute < 10 ? $"0{Minute}" : $"{Minute}";
                string second = Second < 10 ? $"0{Second}" : $"{Second}";
                return isChinese ? Day == 0 ? $"{hour}时{minute}分{second}秒" : $"{day}天{hour}时{minute}分{second}秒" : Day == 0 ? $"{hour}:{minute}:{second}" : $"{day}:{hour}:{minute}:{second}";
            }
        }

        public static TimeSpan GetTimeSpan(this long time, bool isMs = false, bool isFuture = false)
        {
            //#if UNITY_EDITOR
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0)); // 当地时区
                                                                                                                        //#else
                                                                                                                        //        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 8, 0, 0)); // 当地时区
                                                                                                                        //#endif 
            DateTime datetime = isMs ? startTime.AddMilliseconds(time) : startTime.AddSeconds(time);

            return isFuture ? datetime - NowTime : NowTime - datetime;
        }
        
     

    }
}