using System;
using System.IO;
using System.Reflection;

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
        public static void Log(LogType logType, string logDescription)
        {
            var line = GetLog(logType, logDescription);
            
            WriteLog(line);
        }

        public static void LogStart(string logDescription)
        {
            Log(LogType.Start, logDescription);
        }

        public static void LogEnd(string logDescription)
        {
            Log(LogType.End, logDescription);
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

        private static string GetLog(LogType logType, string logDescription)
        {
            return $"{DateTime.Now}|{logType}|{logDescription}";
        }
    }
}
