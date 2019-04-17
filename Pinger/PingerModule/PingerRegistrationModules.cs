using Ninject.Modules;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.Modules;
using Pinger.Protocols;
using ILogger = Pinger.Logger.ILogger;

namespace Pinger.PingerModule
{
    class PingerRegistrationModules:NinjectModule
    {
        public override void Load()
        {
            Bind<IProtocol>().To<HttpProtocol>();
            Bind<IProtocol>().To<TcpProtocol>();
            Bind<IProtocol>().To<IcmpProtocol>();
            Bind<IConfigReader>().To<ConfigurationReader>();
            Bind<IGui>().To<ConsoleGui>();
            Bind<ILogger>().To<Logger.Logger>();
        }
    }
}
