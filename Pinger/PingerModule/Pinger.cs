using System;
using System.Timers;
using Ninject.Infrastructure.Language;
using Pinger.ConfigurationModule;

namespace Pinger.PingerModule
{
    class Pinger:IPinger
    {
        private int _interval;
        private PingProtocol _pProtocol;
        private Logger.ILogger _logger;
        public void WorkProcessed(IConfigReader reader, Logger.ILogger logger)
        {
            _logger = logger;
            try
            {
                reader.GetConfigEntry(Configuration.RefreshRate).Map(x => { x.Start(logger); });
            }
            catch (Exception e)
            {
                logger.Write(e);
            }
        }
    }
}
