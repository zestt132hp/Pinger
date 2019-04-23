using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace Pinger.Logger
{
    interface IConfigurationNlog
    {
        LoggingConfiguration GetLogConfiguration(string logName);
    }
}
