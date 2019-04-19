using System;
using System.Net;
using Pinger.ConfigurationModule;
using Pinger.Modules;
using Pinger.PingerModule;

namespace Pinger.Protocols
{
    internal sealed class ProtocolCreator
    {
        public static PingProtocol CreateProtocol(CustomConfigAttribute data)
        {
            PingProtocol pProtocol;
            switch (data.Protocol?.ToString().ToLowerInvariant())
            {
                case "http":
                {
                    HttpStatusCode code;
                    if (HttpStatusCode.TryParse(data.HttpCode?.ToString(), out code))
                    {
                        pProtocol = new PingProtocol(new HttpProtocol(data.Host.ToString(), code));
                        pProtocol.SetInterval(data.Interval?.ToString());
                        return pProtocol;
                    }
                    else
                    {
                        pProtocol = new PingProtocol(new HttpProtocol(data.Host.ToString()));
                        pProtocol.SetInterval(data.Interval?.ToString());
                        return pProtocol;
                    }
                }
                case "icmp":
                {
                    pProtocol = new PingProtocol(new IcmpProtocol() {Host = data.Host.ToString()});
                    pProtocol.SetInterval(data.Interval.ToString());
                    return pProtocol;

                }
                case "tcp/ip":
                case "tcp":
                case "ip":
                    int port;
                    pProtocol = Int32.TryParse(data.Port.ToString(), out port)
                        ? new PingProtocol(new TcpProtocol(data.Host.ToString()) {Port = port})
                        : new PingProtocol(new TcpProtocol(data.Host.ToString()));
                    return pProtocol;
                default:
                    throw new NotImplementedException(
                        "Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
