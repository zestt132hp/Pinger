using System;
using System.Net;
using Pinger.ConfigurationModule;
using Pinger.Modules;

namespace Pinger.Protocols
{
    sealed class ProtocolCreator
    {
        public static IProtocol CreateProtocol(CustomConfigAttribute data)
        {
            switch (data.Protocol?.ToString().ToLowerInvariant())
            {
                case "http":
                    int interval;
                    short httpcode;
                    HttpProtocol protocol;
                    if (Int32.TryParse(data.Interval?.ToString(), out interval) &&
                        Int16.TryParse(data.HttpCode?.ToString(), out httpcode))
                    {
                        protocol = new HttpProtocol((String) data.Host) {StatusCode = httpcode, Interval = interval };
                    }
                    else
                        protocol = new HttpProtocol((String) data.Host);
                    return protocol;
                case "icmp":
                    return new IcmpProtocol() {Host = (String) data.Host};
                case "tcp/ip":
                case"tcp":
                case "ip":
                    int port;
                    if (Int32.TryParse(data.Interval.ToString(), out interval) &&
                        Int32.TryParse(data.Port.ToString(), out port))
                    {
                        return new TcpProtocol() {Host = (String) data.Host, Interval = interval, Port = port};
                    }
                    else
                        return new TcpProtocol() {Host = (String) data.Host};
                default:throw new NotImplementedException("Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
