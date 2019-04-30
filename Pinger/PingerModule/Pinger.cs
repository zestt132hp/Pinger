using System;
using System.Timers;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public class Pinger : IPinger
    {
        private Timer _timer;
        private IProtocol _protocol;
        private Int32 _interval = 5;
        private ILogger _logger;
        public IProtocol Protocol => _protocol;

        public Pinger(IProtocol protocol, ILogger logger)
        {
            if(protocol == null || logger == null)
                throw new NullReferenceException(nameof(Pinger));
            _protocol = protocol;
            _logger = logger;
        }

        public Int32 Interval
        {
            get => _interval;
            set
            {
                if (value > 0)
                    _interval = value;
            }
        }

        public void StopWork()
        {
            StopTimer();
        }

        public void SetInterval(String value)
        {
            Int32 tmp;
            if (Int32.TryParse(value, out tmp))
                _interval = tmp;
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(Interval);
                _timer.Elapsed += OnTimedEvent;
            }
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            String message = $"Host: {Protocol.Host}\n Result: ";
            message += Protocol.SendRequest(_logger).IsSucces ? "OK" : "FAILED";
            message += "\n Message From Process: " + Protocol.Message;
            _logger.Write(message);
        }

        public void StopTimer()
        {
            if (_timer == null)
                return;
            _timer.Stop();
            _timer.Dispose();
        }

        public void StartWork()
        {
            StartTimer();
        }
    }
}
