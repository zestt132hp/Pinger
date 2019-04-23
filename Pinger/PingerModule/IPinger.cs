using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    interface IPinger
    {
        void StartWork(ILogger logger);
        void SetInterval(string value);
    }
}
