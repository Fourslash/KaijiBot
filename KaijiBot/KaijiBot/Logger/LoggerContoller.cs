using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaijiBot.Logger
{
    class LoggerContoller
    {
        public static Logger ProxyLogger = new Logger("Proxy", LogLevelEnum.Debug);
        public static Logger ProcessLogger = new Logger("Process", LogLevelEnum.Verbose);
    }
}
