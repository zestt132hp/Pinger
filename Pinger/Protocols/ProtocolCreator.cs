using System;
using System.Net;
using Pinger.ConfigurationModule;

namespace Pinger.Protocols
{
    public sealed class ProtocolCreator
    {
        public static PingerModule.IPinger CreateProtocol(CustomConfigAttribute data)
        {
            PingerModule.Pinger pProtocol;
            switch (data.Protocol?.ToString().ToLowerInvariant())
            {
                case "http":
                {
                    HttpStatusCode code;
                    if (Enum.TryParse(data.HttpCode?.ToString(), out code))
                    {
                        pProtocol = new PingerModule.Pinger() {Protocol = new HttpProtocol(data.Host.ToString(), code)};
                        pProtocol.SetInterval(data.Interval?.ToString());
                        return pProtocol;
                    }
                    else
                    {
                        pProtocol = new PingerModule.Pinger() {Protocol = new HttpProtocol(data.Host.ToString())};
                        pProtocol.SetInterval(data.Interval?.ToString());
                        return pProtocol;
                    }
                }
                case "icmp":
                {
                    pProtocol = new PingerModule.Pinger() {Protocol = new IcmpProtocol(data.Host.ToString())};
                    pProtocol.SetInterval(data.Interval.ToString());
                    return pProtocol;

                }
                case "tcp/ip":
                case "tcp":
                case "ip":
                    pProtocol = new PingerModule.Pinger() {Protocol = new TcpProtocol(data.Host.ToString())};
                    pProtocol.SetInterval(data.Interval.ToString());
                    return pProtocol;
                default:
                    throw new NotImplementedException(
                        "Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
