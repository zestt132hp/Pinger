using System;
using Ninject.Infrastructure.Language;

namespace Pinger.GUI
{
    internal class ConsoleInputData : IInputsUi
    {
        public String[] ValuesFromUi()
        {
            return Console.ReadLine()?.Split(' ');
        }

        public string VerifyString(string command, params string[] simbols)
        {
            if (String.IsNullOrEmpty(command))
                return null;
            simbols.Map(x =>
            {
                if (command.Contains(x))
                    command = command.Remove(command.IndexOf(x, StringComparison.Ordinal), 1);
            });/*
            if (command.Contains(simbol) || command.Contains(simbol))
            {
                command = command.Remove(command.IndexOf('['), 1);
                command = command.Remove(command.IndexOf(']'), 1);
            }*/
            return command;
        }
    }
    interface IInputsUi
    {
        String[] ValuesFromUi();
        String VerifyString(string inputStr, params string[] simbols);
    }
}
