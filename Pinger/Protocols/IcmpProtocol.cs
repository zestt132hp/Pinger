using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class IcmpProtocol:Protocol, IProtocol
    {
        public void SendRequest(ILogger loger)
        {
            throw new NotImplementedException();
        }
    }
}
