using Ninject;
using Ninject.Modules;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.UI;

namespace Pinger.PingerModule
{
    public class PingerRegistrationModules:NinjectModule
    {
        private static IKernel _kernel;
        public override void Load()
        {
            Bind<IConfigWorker>().To<ConfigurationWorker>().WithConstructorArgument("PingerConfiguration.config");
            Bind<IConfigurationNlog>().To<NlogConfiguration>();
            Bind<IPingerProcessor>().To<PingerProcessor>();
            Bind<IUi>().To<ConsoleWorkProcessUi>();
            Bind<IConsoleOutputUi>().To<ConsoleOutputOutputMessage>();
            Bind<IInputsUi>().To<ConsoleInputData>();
            Bind<ILogger>().To<Logger.Logger>().WithConstructorArgument("");
        }
        public static IKernel GetKernel()
        {
            return _kernel ?? (_kernel = new StandardKernel(new PingerRegistrationModules()));
        }
    }
    
}
