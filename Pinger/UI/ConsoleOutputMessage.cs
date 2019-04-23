using System;

namespace Pinger.UI
{
    interface IConsoleOutputUi
    {
        void PrintMessage(string message);
        void PrintMessage(string message, string tab, bool endRow);
    }


    internal class ConsoleOutputOutputMessage : IConsoleOutputUi
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void PrintMessage(string message, string tab, bool endRow)
        {
            if(endRow)
                Console.WriteLine(tab+message+tab);
            else
                Console.WriteLine(tab + message);
        }
    }
}
