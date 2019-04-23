using System;
using Ninject.Infrastructure.Language;
using Pinger.ConfigurationModule;
using Pinger.Logger;

namespace Pinger.PingerModule
{
    class PingerProcessor:IPinger
    {
        private IConfigWorker _configWorker;

        public PingerProcessor(IConfigWorker confWorker)
        {
            if (confWorker != null)
                _configWorker = confWorker;
            else
                throw new NullReferenceException(nameof(PingerProcessor));
        }

        public void StartWork(ILogger logger)
        {
            _configWorker.GetFromConfig().Map(x=>x.Value.StartWork(logger));
        }

        public void StopWork()
        {
            _configWorker.GetFromConfig().Map(x=>x.Value.StopTimer());
        }
    }
}
