using System;
using System.Net;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.PingerModule;

namespace Pinger.Protocols
{
    public sealed class ProtocolCreator
    {
        private readonly IProtocolFactory _factory;
        private readonly ILogger<Exception> _excLogger;
        private readonly ILogger<String> _logger;

        public ProtocolCreator(IProtocolFactory factory, ILogger<String> logger, ILogger<Exception> excLogger)
        {
            _factory = factory ?? throw new NullReferenceException(nameof(ProtocolCreator));
            _logger = logger ?? throw new NullReferenceException(nameof(ProtocolCreator) + nameof(ILogger<String>));
            _excLogger = excLogger ??
                         throw new NullReferenceException(nameof(ProtocolCreator) + nameof(ILogger<Exception>));
        }

        public IPinger CreateProtocol(CustomConfigAttribute data)
        {
            IPinger pProtocol;
            switch (data.Protocol.ToLowerInvariant())
            {
                case "http":
                {
                    HttpStatusCode code;
                    if (Enum.TryParse(data.HttpCode, out code))
                    {
                        pProtocol = new PingerModule.Pinger(_factory.CreateHttpProtocol(data.Host, code), _excLogger, _logger);
                        pProtocol.SetInterval(data.Interval);
                        return pProtocol;
                    }
                    else
                    {
                        pProtocol = new PingerModule.Pinger(_factory.CreateHttpProtocol(data.Host), _excLogger, _logger);
                        pProtocol.SetInterval(data.Interval);
                        return pProtocol;
                    }
                }
                case "icmp":
                {
                    pProtocol = new PingerModule.Pinger(_factory.CreateIcmpProtocol(data.Host), _excLogger, _logger);
                    pProtocol.SetInterval(data.Interval);
                    return pProtocol;
                }
                case "tcp/ip":
                case "tcp":
                case "ip":
                {
                    pProtocol = new PingerModule.Pinger(_factory.CreaTcpProtocol(data.Host), _excLogger, _logger);
                    pProtocol.SetInterval(data.Interval);
                    return pProtocol;
                }
                default:
                {
                    throw new NotImplementedException(
                        "Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
                }
            }
        }
    }
}
