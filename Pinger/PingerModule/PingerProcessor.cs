using System;
using System.Linq;
using Pinger.ConfigurationModule;
using Pinger.Logger;

namespace Pinger.PingerModule
{
    public class PingerProcessor:IPingerProcessor
    {
        private readonly IConfigWorker _configWorker;

        public PingerProcessor(IConfigWorker confWorker)
        {
            if (confWorker != null)
                _configWorker = confWorker;
            else
                throw new NullReferenceException(nameof(PingerProcessor));
        }

        public void StartWork(ILogger logger)
        {
            _configWorker.GetFromConfig().Values.AsParallel().ForAll(x=>x.StartWork(logger));
        }

        public void StopWork()
        {
            _configWorker.GetFromConfig().Values.AsParallel().ForAll(x => x.StopWork());
        }

        public int Interval { get; set; }
        public void StartWork(int index, ILogger log)
        {
            if(_configWorker.GetFromConfig().ContainsKey(index))
                _configWorker.GetFromConfig()[index].StartWork(log);
            else
                throw new IndexOutOfRangeException(nameof(StartWork));
        }
    }
}
