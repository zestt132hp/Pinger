using System;

namespace Pinger.UI
{
    public class ConsoleOutputOutputMessage : IConsoleOutputUi
    {
        public void PrintMessage(String message)
        {
            Console.WriteLine(message);
        }

        public void PrintMessage(String message, String tab, Boolean endRow)
        {
            if(endRow)
                Console.WriteLine(tab+message+tab);
            else
                Console.WriteLine(tab + message);
        }
    }
}
