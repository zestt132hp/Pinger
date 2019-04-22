using System;
using System.Linq;
using System.Timers;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    internal class Pinger : IPinger
    {
        private System.Timers.Timer _timer;
        private IProtocol _protocol;
        private static Int32 _interval = 5;
        private Logger.ILogger _logger;
        public IProtocol Protocol {
            get { return _protocol; }
            set { _protocol = value; }
        }
        public Int32 Interval { get; private set; } = _interval;
        public void SetInterval(string value)
        {
            Int32 tmp;
            if (Int32.TryParse(value, out tmp))
                _interval = tmp;
        }

        public void StartTimer()
        {
            _timer = new System.Timers.Timer(Interval);
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
            _timer.Stop();
            _timer.Dispose();
        }

        public void StartWork( ILogger logger)
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
