using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
            ConfigurationModule.IConfigReader reader = new ConfigurationReader("PingerConfiguration.config");
            reader.GetConfigEntry(Configuration.RefreshRate);
        }
    }
}
