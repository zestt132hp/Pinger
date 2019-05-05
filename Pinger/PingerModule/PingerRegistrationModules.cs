using System;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.Protocols;
using Pinger.UI;

namespace Pinger.PingerModule
{
    public class PingerRegistrationModules:NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigWorker>().To<ConfigurationWorker>().InSingletonScope().WithConstructorArgument("PingerConfiguration.config");
            Bind<ProtocolCreator>().ToSelf().InSingletonScope();
            Bind<IConfigurationNlog>().To<NlogConfiguration>();
            Bind<IPingerProcessor>().To<PingerProcessor>().WithConstructorArgument("confWorker");
            Bind<IUi>().To<ConsoleWorkProcessUi>();
            Bind<IConsoleOutputUi>().To<ConsoleOutputOutputMessage>();
            Bind<IInputsUi>().To<ConsoleInputData>();
            Bind<ILogger<String>>().To<Logger.Logger>().WithConstructorArgument(DateTime.Now.ToShortDateString());
            Bind<ILogger<Exception>>().To<ExceptionLogger>()
                .WithConstructorArgument(DateTime.Now.ToShortDateString());
            Bind<IProtocolFactory>().ToFactory(()=>new TypeMatchingArgumentInheritanceInstanceProvider());
        }
    }
    
}
