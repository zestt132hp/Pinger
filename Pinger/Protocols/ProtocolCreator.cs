using System;
using System.Net;
using Pinger.ConfigurationModule;
using Pinger.Modules;
using Pinger.PingerModule;

namespace Pinger.Protocols
{
    internal sealed class ProtocolCreator
    {
        public static PingerModule.Pinger CreateProtocol(CustomConfigAttribute data)
        {
            PingerModule.Pinger pProtocol;
            switch (data.Protocol?.ToString().ToLowerInvariant())
            {
                case "http":
                {
                    HttpStatusCode code;
                    if (HttpStatusCode.TryParse(data.HttpCode?.ToString(), out code))
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
                    pProtocol = new PingerModule.Pinger() {Protocol = new IcmpProtocol() {Host = data.Host.ToString()}};
                    pProtocol.SetInterval(data.Interval.ToString());
                    return pProtocol;

                }
                case "tcp/ip":
                case "tcp":
                case "ip":
                    int port;
                    pProtocol = Int32.TryParse(data.Port.ToString(), out port)
                        ? new PingerModule.Pinger() {Protocol = new TcpProtocol(data.Host.ToString()) {Port = port}}
                        : new PingerModule.Pinger() {Protocol = new TcpProtocol(data.Host.ToString())};
                    return pProtocol;
                default:
                    throw new NotImplementedException(
                        "Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
