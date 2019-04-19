using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.ConfigurationModule;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    interface IPinger
    {
        void Ping(IConfigReader host, Logger.ILogger logger);
    }
}
