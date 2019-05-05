using System;
using System.Timers;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public class Pinger : IPinger
    {
        private Timer _timer;
        private readonly IProtocol _protocol;
        private Int32 _interval = 5;
        public IProtocol Protocol => _protocol;
        private readonly ILogger<Exception> _excLogger; 
        private readonly ILogger<String> _logger;

        public Pinger(IProtocol protocol, ILogger<Exception> excLogger, ILogger<string> logger)
        {
            _protocol = protocol ?? throw new NullReferenceException(nameof(Pinger));
            _excLogger = excLogger;
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
            message += Protocol.SendRequest<Exception>(_excLogger).IsSucces ? "OK" : "FAILED";
            message += "\n Message From Process: " + Protocol.Message;
            _logger.Write(message);
        }

        public void StopTimer()
        {
            if (_timer == null)
                throw new NullReferenceException(nameof(_timer));
            _timer.Stop();
        }

        public void StartWork()
        {
            StartTimer();
        }
    }
}
