#if !DEBUG || PROFILE_SVELTO
#define DISABLE_DEBUG
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using AFForUnity.Kits.DevlpKit.Helpers.StringHelpers;
using AFForUnity.Kits.DevlpKit.Supplements.Collections;
#if NETFX_CORE
using Windows.System.Diagnostics;
#else
#endif

namespace AFForUnity.Kits.DevlpKit.Tools.Log
{
    public static class Console
    {
        static readonly ThreadLocal<StringBuilder> s_StringBuilder = new ThreadLocal<StringBuilder>
            (() => new StringBuilder(256));

        static readonly FasterList<ILogger> s_Loggers = new FasterList<ILogger>();

        static readonly ILogger s_StandardLogger = new SimpleLogger();

        static Console()
        {
            AddLogger(s_StandardLogger);
        }

        public static void SetLogger(ILogger log)
        {
            s_Loggers[0] = log;

            log.OnLoggerAdded();
        }

        public static void AddLogger( ILogger log)
        {
            s_Loggers.Add(in log);

            log.OnLoggerAdded();
        }

        public static void Log(string txt) { InternalLog(txt, LogType.Log); }

        public static void LogError(string txt, Dictionary<string, string> extraData = null)
        {
            string toPrint;

            lock (s_StringBuilder)
            {
                s_StringBuilder.Value.Length = 0;
                s_StringBuilder.Value.Append("-!!!!!!-> ").Append(txt);

                toPrint = s_StringBuilder.ToString();
            }

            InternalLog(toPrint, LogType.Error, null, extraData);
        }

        public static void LogException(Exception exception, string message = null
                                      , Dictionary<string, string> extraData = null)
        {
            if (extraData == null)
                extraData = new Dictionary<string, string>();

            string toPrint = "-!!!!!!-> ";

            Exception tracingE = exception;
            while (tracingE.InnerException != null)
            {
                tracingE = tracingE.InnerException;

                InternalLog("-!!!!!!-> ", LogType.Error, tracingE);
            }

            if (message != null)
            {
                lock (s_StringBuilder)
                {
                    s_StringBuilder.Value.Length = 0;
                    s_StringBuilder.Value.Append(toPrint).Append(message);

                    toPrint = s_StringBuilder.ToString();
                }
            }

            //the goal of this is to show the stack from the real error
            InternalLog(toPrint, LogType.Exception, exception, extraData);
        }

        public static void LogWarning(string txt)
        {
            string toPrint;

            lock (s_StringBuilder)
            {
                s_StringBuilder.Value.Length = 0;
                s_StringBuilder.Value.Append("------> ").Append(txt);

                toPrint = s_StringBuilder.ToString();
            }

            InternalLog(toPrint, LogType.Warning);
        }

        [Conditional("DEBUG")]
        public static void LogDebug(string txt) { InternalLog(txt, LogType.LogDebug); }

        [Conditional("DEBUG")]
        public static void LogDebug<T>(string txt, T extradebug)
        {
            InternalLog(txt.FastConcat(extradebug.ToString()), LogType.LogDebug);
        }

        public static void LogDebugWarning(string txt)
        {
            InternalLog(txt, LogType.Warning); 
        }

        /// <summary>
        /// this class methods can use only InternalLog to log and cannot use the public methods, otherwise the
        /// stack depth will break 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="type"></param>
        /// <param name="e"></param>
        /// <param name="extraData"></param>
        static void InternalLog(string txt, LogType type, Exception e = null
                              , Dictionary<string, string> extraData = null)
        {
            for (int i = 0; i < s_Loggers.count; i++)
            {
                s_Loggers[i].Log(txt, type, e, extraData);
            }
        }
    }
}