using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.Logger;

namespace Pinger.Protocols
{
    internal interface IProtocol
    {
        void SendRequest(ILogger loger);
    }
}
