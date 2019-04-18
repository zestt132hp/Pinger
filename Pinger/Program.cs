using System;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.PingerModule;

namespace Pinger
{
    class Program
    {
        private static IKernel ninjectKernel;
        private static Logger.ILogger logger;
        static Program()
        {
            ninjectKernel = new StandardKernel(new PingerRegistrationModules());
            logger = ninjectKernel.Get<Logger.ILogger>();
        }

        static void Main(string[] args)
        {
            IGui gui = ninjectKernel.Get<IGui>();
            gui.Run();
            /*IPinger pinger = ninjectKernel.Get<IPinger>();
            pinger.Ping(ninjectKernel.Get<IConfigReader>(), logger);*/
        }

        internal static void Run(object stateInfo)
        {
            Console.WriteLine("Начало операции...");
            IPinger pinger = ninjectKernel.Get<IPinger>();
            pinger.Ping(ninjectKernel.Get<IConfigReader>(), logger);
        }
    }
}
 