using System;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    class PingProtocol
    {
        private IProtocol _protocol;
        private static Int32 _interval = 5;
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

        public void Ping(ILogger logger)
        {
           _protocol.SendRequest(logger);
        }
    }
}
