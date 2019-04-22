using System;
namespace Pinger.GUI
{
    internal class ConsoleInputData : IInputsUi
    {
        public String[] ValuesFromUi()
        {
            return Console.ReadLine()?.Split(' ');
        }
    }
    interface IInputsUi
    {
        String[] ValuesFromUi();
    }
}
