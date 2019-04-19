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
        void WorkProcessed(IConfigReader host, Logger.ILogger logger);
    }
}
