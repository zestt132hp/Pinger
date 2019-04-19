using System;
using System.Net;
using System.Net.NetworkInformation;
using Ninject.Modules;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.Logger;
using Pinger.Modules;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    class PingerRegistrationModules:NinjectModule
    {
        public override void Load()
        {
            Bind<IProtocol>().To<HttpProtocol>();
            Bind<IProtocol>().To<TcpProtocol>();
            Bind<IProtocol>().To<IcmpProtocol>();
            Bind<IConfigReader>().To<ConfigurationReader>().WithConstructorArgument("PingerConfiguration.config");
            Bind<IPinger>().To<PingerModule.Pinger>();
            Bind<IGui>().To<ConsoleGui>();
            Bind<Logger.ILogger>().To<Logger.Logger>().WithConstructorArgument("logger");
        }
    }
    
}
