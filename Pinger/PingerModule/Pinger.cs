using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninject.Infrastructure.Language;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;
using ILogger = Castle.Core.Logging.ILogger;

namespace Pinger.PingerModule
{
    class Pinger:IPinger
    {
        private Timer timer;
        private int interval;
        public void Ping(IConfigReader reader, Logger.ILogger logger)
        {
            try
            {
                reader.GetConfigEntry(Configuration.RefreshRate);
            }
            catch (Exception e)
            {
                logger.Write(e);
            }
        }

        public void StartTimer(int interval)
        {

        }
        public void StopTimer() { }
    }
}
