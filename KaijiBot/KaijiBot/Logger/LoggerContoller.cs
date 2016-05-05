using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Logger
{
    class LoggerContoller
    {
#if (DEBUG)
        static LogLevelEnum logLevel = LogLevelEnum.Verbose;
#else
        static LogLevelEnum logLevel = LogLevelEnum.Info;
#endif


        public static Logger ProxyLogger = new Logger("Proxy", logLevel);
        public static Logger ProcessLogger = new Logger("Process", logLevel);
        public static Logger GameLogger = new Logger("GameLogger", logLevel);
    }
}
