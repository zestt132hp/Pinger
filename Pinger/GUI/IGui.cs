using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pinger.PingerModule;

namespace Pinger.GUI
{
    interface IGui
    {
        void ShowMessage(string message);
        void RunGui();
    }
}
