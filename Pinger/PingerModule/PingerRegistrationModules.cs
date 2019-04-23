using System;
using System.Net;
using System.Net.NetworkInformation;
using Ninject;
using Ninject.Modules;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.Logger;
using Pinger.Modules;
using Pinger.Protocols;
using Pinger.UI;

namespace Pinger.PingerModule
{
    class PingerRegistrationModules:NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigWorker>().To<ConfigurationWorker>().WithConstructorArgument("PingerConfiguration.config");
            Bind<IPinger>().To<Pinger>();
            Bind<IUi>().To<ConsoleWorkProcessUi>();
            Bind<IConsoleUi>().To<ConsoleOutputMessage>();
            Bind<IInputsUi>().To<ConsoleInputData>();
            Bind<Logger.ILogger>().To<Logger.Logger>().WithConstructorArgument("logger");
        }
    }
    
}
