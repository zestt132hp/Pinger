using System;
using System.Timers;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    class PingProtocol
    {
        private System.Timers.Timer _timer;
        private IProtocol _protocol;
        private static Int32 _interval = 5;
        private Logger.ILogger _logger;
        public IProtocol ProtocolInfo => _protocol;

        public PingProtocol(IProtocol protocol)
        {
            if (protocol == null)
                throw new NullReferenceException(nameof(IProtocol));
            _protocol = protocol;
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
            string message = $"Host: {ProtocolInfo.Host}\n Result: ";
            message += ProtocolInfo.SendRequest(_logger).IsSucces? "OK" : "FAILED";
            message += "\n Message From Process: " + ProtocolInfo.Message;
            _logger.Write(message);
        }
        public void StopTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }
        public void Start(ILogger logger)
        {
            _logger = logger;
            StartTimer();
        }
    }
}
