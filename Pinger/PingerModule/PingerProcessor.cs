using System;
using System.Linq;
using System.Threading;
using Pinger.ConfigurationModule;

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

        public void Ping()
        {
            _configWorker.GetFromConfig().Values.AsParallel().ForAll(x=>x.StartWork());
        }

        public void StopPing()
        {
            _configWorker.GetFromConfig().Values.AsParallel().ForAll(x => x.StopWork());
            Thread.Sleep(2000);
        }

        public void StopPing(Int32 index)
        {
            if (_configWorker.GetFromConfig().ContainsKey(index))
                _configWorker.GetFromConfig()[index].StopWork();
            else
                throw new IndexOutOfRangeException(nameof(index));
        }

        public void Ping(Int32 index)
        {
            if(_configWorker.GetFromConfig().ContainsKey(index))
                _configWorker.GetFromConfig()[index].StartWork();
            else
                throw new IndexOutOfRangeException(nameof(Ping));
        }
    }
}
