using System;
using System.Linq;
using Pinger.GUI;

namespace Pinger.PingerModule
{
    internal sealed class ConsoleGui: IGui
    {
        private String helloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку 'pinger -help'";
        private string exampleAddParametersInConfig = "Пример добавления в конфигурационный файл настройки для хоста:\n Section_N:{Host:\"google.com\"\n Interval:\"10\",\n Protocol:\"ICMP\"}\n";
        private string exampleAddHost = "";
        private string addMessage = "Введите хост --[Host] --[Interval/empty] --[Protocol] \n";

        public String Message(String[] keyPress)
        {
            string command = "";
            string option = "";
            if (keyPress.Length > 2)
            {
                option += keyPress[0] +" "+ keyPress[1];
                command = keyPress.Skip(2).Aggregate((x, y) => x + ' ' + y);
            }
            else
                option = keyPress.Aggregate((x, y) => x + " " + y);
                switch (option)
                {
                case KeyOptions.Add:
                    return addMessage;
                case KeyOptions.Quit:
                    return KeyOptions.Quit;
                case KeyOptions.Start:
                    return helloMessage;
                case KeyOptions.Help:
                {
                    Console.WriteLine(KeyOptions.GetHelpOptions().Aggregate((a, b) => a + "\n" + b));
                    Message(Console.ReadLine().Split(' '));
                    return null;
                }
                default:
                    return "Неизвестная опция! Повторите ввод или наберите 'pinger -help' для вызова справки \n";
            }
        }
        public void Run()
        {
            Start(this);
        }
        private void Start(IGui gui)
        {
            PrintStartMessage(gui);
        }

        private string Input()
        {
            return Console.ReadLine();
        }

        private void PrintStartMessage(GUI.IGui gui)
        {
            string slash = "";
            for (int x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            Console.WriteLine(slash);
            Console.WriteLine("\n" + gui.Message("pinger start".Split(' ')) + "\n");
            Console.WriteLine(slash);
            gui.Message(Console.ReadLine()?.Split(' '));
        }
    }
    internal struct KeyOptions
    {
        public const String Help = "pinger -help";
        public const String Add = "pinger -add";
        public const String Quit = "pinger -q";
        public const String Remove = "pinger -r";
        public const String Show = "pinger -s";
        public const String Start = "pinger start";

        public static string[] GetHelpOptions()
        {
            return new[] {
                Add + " - позволяет добавить имя хоста в конфигурационный файл \n \t Пример " + Add + " [wwww.yandex.ru 5 ICMP]",
                Quit + " - Завершает работу приложения",
                Remove + " [№]  - номер хоста",
                Show  + " Отображает сохраненные хосты из файла конфигураций"};
        }
    }
}
