using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;
using ILogger = Castle.Core.Logging.ILogger;

namespace Pinger.PingerModule
{
    class PingerWorker:IPingerWorker
    {
        public bool Ping(IProtocol host)
        {
            throw new NotImplementedException();
        }

        public void StartTimer()
        {

        }
        public void StopTimer() { }
    }
}
