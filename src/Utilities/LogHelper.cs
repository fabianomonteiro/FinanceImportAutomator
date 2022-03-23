using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public enum LogType
    {
        On,
        Off,
        Start,
        End,
        Info,
        Warning,
        Error
    }

    public static class LogHelper
    {
        public static void Log(LogType logType, string logDescription, params string[] args)
        {
            var line = GetLog(logType, logDescription, args);

            WriteLog(line);
        }

        public static void LogStart(string logDescription, params string[] args)
        {
            Log(LogType.Start, logDescription, args);
        }

        public static void LogEnd(string logDescription, params string[] args)
        {
            Log(LogType.End, logDescription, args);
        }

        public static void LogError(string logDescription, Exception exception)
        {
            var line = GetLog(LogType.Error, logDescription);

            line += $"|{exception.Message}";

            WriteLog(line);
        }

        private static void WriteLog(string line)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(directory, "Log.txt");

            File.AppendAllText(file, line + Environment.NewLine);
        }

        private static string GetLog(LogType logType, string logDescription, params string[] args)
        {
            StringBuilder stringBuilder = new StringBuilder($"{DateTime.Now}|{logType}|{logDescription}");

            if (args != null)
            {
                foreach (var item in args)
                {
                    stringBuilder.Append($"|{item}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
