using System;
using System.Net;
using System.Net.NetworkInformation;
using Pinger.ConfigurationModule;
using Pinger.Modules;

namespace Pinger.Protocols
{
    sealed class ProtocolCreator
    {
        public static PingerProtocol CreateProtocol(CustomConfigAttribute data)
        {
            switch (data.Protocol?.ToString().ToLowerInvariant())
            {
                case "http":
                    double interval;
                    short httpcode;
                    HttpProtocol protocol;
                    if (Double.TryParse(data.Interval?.ToString(), out interval) &&
                        Int16.TryParse(data.HttpCode?.ToString(), out httpcode))
                    {
                        return new PingerProtocol(){ Interval = interval};
                    }
                    return null;
                case "icmp":
                    return null;//new IcmpProtocol() {Host = (String)data.Host};
                case "tcp/ip":
                case"tcp":
                case "ip":
                    int port;
                    if (Double.TryParse(data.Interval.ToString(), out interval) &&
                        Int32.TryParse(data.Port.ToString(), out port))
                    {
                        return null; //return new TcpProtocol() {Host = (String) data.Host, Port = port};
                    }
                    else
                        return null;// new TcpProtocol() {Host = (String) data.Host};
                default:throw new NotImplementedException("Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
