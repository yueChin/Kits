using System;
using System.Collections.Generic;
using Kits.DevlpKit.Helpers.StringHelpers;

namespace Kits.DevlpKit.Tools.ConsoleLog
{
    public class SimpleLogger : ILogger
    {
        public void Log(string txt, LogType type = LogType.Log, Exception e = null, Dictionary<string, string> data = null)
        {
            string dataString = string.Empty;

            string stack = null;

            if (e != null)
                stack = e.StackTrace;

            if (data != null)
                dataString = DataToString.DetailString(data);

            switch (type)
            {
                case LogType.Log:
                    SystemLog(txt);
                    break;
                case LogType.Warning:
                    SystemLog(txt);
                    break;
                case LogType.Error:
                case LogType.LogDebug:
                case LogType.Exception:
                    SystemLog(txt.FastConcat("</color> ", Environment.NewLine, stack)
                        .FastConcat(Environment.NewLine, dataString));
                    break;
            }
        }

        public void OnLoggerAdded()
        {
        }

        public static void SystemLog(string txt)
        {
            System.Console.WriteLine(txt);
        }
    }
}