using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class PingerProtocol
    {
        public Double Interval { get; set; }
        public void Ping<T>(IProtocol protocol, ILogger logger)
        {
           protocol.SendRequest(logger);
        }
    }
}
