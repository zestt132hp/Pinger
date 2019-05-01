using System;

namespace Pinger.UI
{
    interface IConsoleOutputUi
    {
        void PrintMessage(String message);
        void PrintMessage(String message, String tab, Boolean endRow);
    }

}
