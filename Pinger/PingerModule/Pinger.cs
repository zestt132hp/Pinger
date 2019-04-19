using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Ninject.Infrastructure.Language;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;
using ILogger = Castle.Core.Logging.ILogger;

namespace Pinger.PingerModule
{
    class Pinger:IPinger
    {
        private System.Timers.Timer _timer;
        private int interval;
        public void Ping(IConfigReader reader, Logger.ILogger logger)
        {
            try
            {
                reader.GetConfigEntry(Configuration.RefreshRate).Map(x=>x.Ping(logger));
            }
            catch (Exception e)
            {
                logger.Write(e);
            }
        }

        public void StartTimer(int interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StopTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
