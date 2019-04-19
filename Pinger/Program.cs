using System;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.PingerModule;

namespace Pinger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IGui gui = new ConsoleGui();
            gui.RunGui();
        }
    }
}
 