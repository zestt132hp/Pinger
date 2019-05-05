using System;
using Pinger.UI;

namespace Pinger
{
    internal class Program
    {
        private static void Main(String[] args)
        {
            IUi ui = new ConsoleWorkProcessUi();
            ui.RunGui();
        }
    }
}
 