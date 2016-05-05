using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Logger
{
    enum LogLevelEnum { Trace = 7, Verbose = 6, Debug = 5, Info = 4, Success = 3, Warn = 2, Error = 1, Critical = 0 }

    class LogLevel
    {
        public string Name { get; set; }
        public LogLevelEnum Level { get; set; }
        public ConsoleColor Color { get; set; }
        public LogLevel(string name, LogLevelEnum level, ConsoleColor color)
        {
            Name = name;
            Level = level;
            Color = color;
        }
    }

    class LogLevels
    {     
        public static LogLevel Trace = new LogLevel("TRACE", LogLevelEnum.Trace, ConsoleColor.Blue);
        public static LogLevel Verbose = new LogLevel("VERBOSE", LogLevelEnum.Verbose, ConsoleColor.Magenta);
        public static LogLevel Debug = new LogLevel("DEBUG", LogLevelEnum.Debug, ConsoleColor.Cyan);
        public static LogLevel Info = new LogLevel("INFO", LogLevelEnum.Info, ConsoleColor.White);
        public static LogLevel Warn = new LogLevel("WARN", LogLevelEnum.Warn, ConsoleColor.Yellow);
        public static LogLevel Success = new LogLevel("SUCCESS", LogLevelEnum.Success, ConsoleColor.Green);
        public static LogLevel Error = new LogLevel("ERROR", LogLevelEnum.Info, ConsoleColor.Red);
        public static LogLevel Critical = new LogLevel("CRITICAL", LogLevelEnum.Info, ConsoleColor.DarkRed);
    }
}
