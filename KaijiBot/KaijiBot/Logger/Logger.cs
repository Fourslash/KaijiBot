using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Logger
{
    


    class Logger
    {
        string Name { get; set; }
        private LogLevelEnum MaximumLogLevel { get; set; }

        public Logger(string name, LogLevelEnum maximumLevel)
        {
            Name = name;
            MaximumLogLevel = maximumLevel;
        }

        public void Trace(string message)
        {
            Log(LogLevels.Trace, message);
        }

        public void Verbose(string message)
        {
            Log(LogLevels.Verbose, message);
        }

        public void Debug(string message)
        {
            Log(LogLevels.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevels.Info, message);
        }

        public void Success(string message)
        {
            Log(LogLevels.Success, message);
        }

        public void Warn(string message)
        {
            Log(LogLevels.Warn, message);
        }

        public void Error(string message)
        {
            Log(LogLevels.Error, message);
        }
        public void Error(Exception ex)
        {
            Log(LogLevels.Error, ex.ToString());
        }

        
        public void Critical(string message)
        {
            Log(LogLevels.Critical, message);
        }
        public void Critical(Exception ex)
        {
            Log(LogLevels.Critical, ex.ToString());
        }



        private void Log(LogLevel level, string message)
        {
            if (level.Level <= MaximumLogLevel)
            {
                var start = string.Format("[{0}] [{1}] [", DateTime.Now, this.Name);
                var end = string.Format("] {0} \n", message);

                var oldColor = Console.ForegroundColor;
                Console.Write(start);
                Console.ForegroundColor = level.Color;
                Console.Write(level.Name);
                Console.ForegroundColor = oldColor;
                Console.Write(end);
            }
        }
    }
}
