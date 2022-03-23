using System;
using System.IO;
using System.Reflection;

namespace FinanceImportAutomator
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
            var file = Path.Combine(Assembly.GetExecutingAssembly().Location, "Log.txt");
            string[] line = new string[] { $"{DateTime.Now}|{logType}|{logDescription}" };

            File.WriteAllLines(file, line);
        }
    }
}
