using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Modules
{
    internal class IcmpProtocol:IProtocol
    {
        public string Host { get; set; }
        //public Double Interval { get; set; }

        public RequestStatus SendRequest(ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
