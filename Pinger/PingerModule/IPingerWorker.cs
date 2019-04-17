using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    interface IPingerWorker
    {
        bool Ping(IProtocol host);
    }
}
