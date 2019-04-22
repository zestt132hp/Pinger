﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.PingerModule;
using Pinger.UI;

namespace Pinger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IUi ui = new ConsoleWorkProcessUi();
            ui.RunGui();
        }
    }
}
 