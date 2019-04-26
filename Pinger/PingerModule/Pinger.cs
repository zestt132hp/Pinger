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
        public IProtocol Protocol {
            get { return _protocol; }
            set { _protocol = value; }
        }

        public Int32 Interval
        {
            get { return _interval; }
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

        public void SetInterval(string value)
        {
            Int32 tmp;
            if (Int32.TryParse(value, out tmp))
                _interval = tmp;
        }

        public void StartTimer()
        {
            _timer = new Timer(Interval);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            string message = $"Host: {Protocol.Host}\n Result: ";
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

        public void StartWork(ILogger logger)
        {
            if (Protocol == null)
                return;
            if (logger == null)
                return;
            _logger = logger;
            StartTimer();
        }
    }
}
