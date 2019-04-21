using System;
using System.Linq;
using System.Threading;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.GUI;

namespace Pinger.PingerModule
{
    internal sealed class ConsoleGui: IGui
    {
        private String helloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку [pinger -help]";
        private string addMessage = "Введите хост --[Host] --[Interval/empty] --[Protocol] \n";
        private Logger.ILogger log;
        private IPinger pinger;
        private IKernel NinjectKernel;
        private IConfigReader reader;
        public ConsoleGui()
        {
            NinjectKernel = new StandardKernel(new PingerModule.PingerRegistrationModules());
            log = NinjectKernel.Get<Logger.ILogger>();
            pinger = NinjectKernel.Get<IPinger>();
            reader = NinjectKernel.Get<IConfigReader>();
        }
        private String OptionsToWork(String[] keyPress)
        {
            string command="";
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
                {
                    if (string.IsNullOrEmpty(command))
                        Console.WriteLine("значение при добавлении не может быть пустым, " +
                                          "\nвызовите справку или ввидите корректное значение для добавления хоста");
                    else
                    {
                        if (command.Contains('[') || command.Contains(']'))
                        {
                            command = command.Remove(command.IndexOf('['), 1);
                            command = command.Remove(command.IndexOf(']'), 1);
                        }
                        reader.AddInConfig(command.Split(' '));
                    }
                    OptionsToWork(Console.ReadLine().Split(' '));
                    return null;
                }
                case KeyOptions.Quit:
                    Console.WriteLine("Приложение завершает свою работу");
                    Thread.Sleep(1000);
                    try
                    {
                        Thread.CurrentThread.Abort();
                    }
                    catch (ThreadAbortException e)
                    {
                        Environment.Exit(0);
                    }
                    return null;
                case KeyOptions.Start:
                    return helloMessage;
                case KeyOptions.Ping:
                {
                    Console.WriteLine(
                        "Начинается переодический опрос хостов из файла конфигураций, результат опроса будет записан в логфайл \n\n");
                    ThreadPool.QueueUserWorkItem(StartPinger);
                        Thread.Sleep(200);
                    Console.WriteLine("введите команду для выхода [pinger -quit]");
                    OptionsToWork(Console.ReadLine()?.Split(' '));
                    return null;
                }
                case KeyOptions.Show:
                {
                    Console.WriteLine(reader.GetHosts().Count > 0
                        ? reader.GetHosts()
                            .Select(x => $"Host: {x.Host} Protocol: {x.ProtocolName}")
                            .Aggregate((x, y) => x + "\n" + y)
                        : "В конфигурационном файле отстувует список хостов, добавьте хост");
                    OptionsToWork(Console.ReadLine().Split(' '));
                    return null;
                }
                case KeyOptions.Help:
                {
                    Console.WriteLine(KeyOptions.GetHelpOptions().Aggregate((a, b) => $"{a}\n{b}")+"\n");
                    OptionsToWork(Console.ReadLine()?.Split(' '));
                    return null;
                }
                default:
                    Console.WriteLine(
                        "Неизвестная опция! Повторите ввод или наберите [pinger -help] для вызова справки \n\n");
                    return OptionsToWork(Console.ReadLine()?.Split(' '));
            }
        }
        private void StartPinger(object state)
        {
            pinger.WorkProcessed(reader, log);
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void RunGui()
        {
            string slash = "";
            for (int x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            Console.WriteLine(slash);
            Console.WriteLine("\n" + OptionsToWork("pinger start".Split(' ')) + "\n");
            Console.WriteLine(slash);
            OptionsToWork(Console.ReadLine()?.Split(' '));
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
                Add + " - позволяет добавить имя хоста в конфигурационный файл \n \n \t Пример для ICMP протокола " + Add + " [wwww.yandex.ru 5 ICMP] \n" + 
                "\t Пример для Http/Https протокола: [www.yandex.ru 16 http 200]\n \t - последняя цифра это статус-код \n" + 
                "\t Пример для tcp/ip протокола: [10.200.224.94:50 15 tcp/ip]\n \t - через двоеточие указывается порт",
                Quit + " - Завершает работу приложения",
                Remove + " [№]  - удаляет хост с указанным номером",
                Show  + " - Отображает сохраненные хосты из файла конфигураций",
                Ping + " - Запускает опрос указанных хостов в конфиг файле"
            };
        }
    }
}
