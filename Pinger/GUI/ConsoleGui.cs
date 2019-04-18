﻿using System;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using Ninject.Infrastructure.Language;
using Pinger.ConfigurationModule;
using Pinger.GUI;

namespace Pinger.PingerModule
{
    internal sealed class ConsoleGui: IGui
    {
        private String helloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку 'pinger -help'";
        private string addMessage = "Введите хост --[Host] --[Interval/empty] --[Protocol] \n";
        private Logger.ILogger log;

        public String Message(String[] keyPress)
        {
            string command;
            string option;
            if (keyPress.Length > 2)
            {
                option = keyPress[0] + " " + keyPress[1];
                command = keyPress.Skip(2).Aggregate((x, y) => x + ' ' + y);
            }
            else
                option = keyPress.Aggregate((x, y) => x + " " + y);
            switch (option)
            {
                case KeyOptions.Add:
                    return addMessage;
                case KeyOptions.Quit:
                    Console.WriteLine("Приложение завершает свою работу");
                    Thread.CurrentThread.Abort();
                    Environment.Exit(0);
                    return null;
                case KeyOptions.Start:
                    return helloMessage;
                case KeyOptions.Ping:
                {
                    Console.WriteLine(
                        "Начинается переодический опрос хостов из файла конфигураций, результат опроса будет записан в логфайл \n\n");
                    ThreadPool.QueueUserWorkItem(Program.Run);
                        Thread.Sleep(200);
                    Console.WriteLine("введите команду для выхода [pinger -quit]");
                    Message(Console.ReadLine()?.Split(' '));
                    return null;
                }
                case KeyOptions.Help:
                {
                    Console.WriteLine(KeyOptions.GetHelpOptions().Aggregate((a, b) => $"{a}\n{b}")+"\n");
                    Message(Console.ReadLine()?.Split(' '));
                    return null;
                }
                default:
                    Console.WriteLine(
                        "Неизвестная опция! Повторите ввод или наберите 'pinger -help' для вызова справки \n\n");
                    return Message(Console.ReadLine()?.Split(' '));
            }
        }

        public void Run()
        {
            string slash = "";
            for (int x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            Console.WriteLine(slash);
            Console.WriteLine("\n" + Message("pinger start".Split(' ')) + "\n");
            Console.WriteLine(slash);
            Message(Console.ReadLine()?.Split(' '));
        }
    }
    internal struct KeyOptions
    {
        public const String Help = "pinger -help";
        public const String Add = "pinger -add";
        public const String Quit = "pinger -quit";
        public const String Remove = "pinger -remove";
        public const String Show = "pinger -show";
        public const String Start = "pinger start";
        public const String Ping = "pinger -ping";

        public static string[] GetHelpOptions()
        {
            return new[] {
                Add + " - позволяет добавить имя хоста в конфигурационный файл \n \n \t Пример " + Add + " [wwww.yandex.ru 5 ICMP] \n",
                Quit + " - Завершает работу приложения",
                Remove + " [№]  - удаляет хост с указанным номером",
                Show  + " - Отображает сохраненные хосты из файла конфигураций",
                Ping + " - Запускает опрос указанных хостов в конфиг файле"
            };
        }
    }
}
