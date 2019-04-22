using System;

namespace Pinger.UI
{
    interface IConsoleUi
    {
        void ShowMessage(string message);
    }


    internal class ConsoleOutputMessage : IConsoleUi
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
