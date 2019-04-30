using System;
using System.Net;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.PingerModule;

namespace Pinger.Protocols
{
    public sealed class ProtocolCreator
    {
        public static IPinger CreateProtocol(CustomConfigAttribute data)
        {
            IPinger pProtocol;
            switch (data.Protocol.ToLowerInvariant())
            {
                case "http":
                {
                    HttpStatusCode code;
                    if (Enum.TryParse(data.HttpCode, out code))
                    {
                        pProtocol = new PingerModule.Pinger(new HttpProtocol(data.Host, code), PingerRegistrationModules.GetKernel().Get<ILogger>());
                        pProtocol.SetInterval(data.Interval);
                        return pProtocol;
                    }
                    else
                    {
                        pProtocol = new PingerModule.Pinger(new HttpProtocol(data.Host), PingerRegistrationModules.GetKernel().Get<ILogger>());
                        pProtocol.SetInterval(data.Interval);
                        return pProtocol;
                    }
                }
                case "icmp":
                {
                    pProtocol = new PingerModule.Pinger(new IcmpProtocol(data.Host), PingerRegistrationModules.GetKernel().Get<ILogger>());
                    pProtocol.SetInterval(data.Interval);
                    return pProtocol;

                }
                case "tcp/ip":
                case "tcp":
                case "ip":
                    pProtocol = new PingerModule.Pinger(new TcpProtocol(data.Host), PingerRegistrationModules.GetKernel().Get<ILogger>());
                    pProtocol.SetInterval(data.Interval);
                    return pProtocol;
                default:
                    throw new NotImplementedException(
                        "Реализовано всего три протокола, пожалуйста убедитесь в правильности ввода");
            }
        }
    }
}
