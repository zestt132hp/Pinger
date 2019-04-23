using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.GUI;
using Pinger.PingerModule;

namespace Pinger.UI
{
    internal sealed class ConsoleWorkProcessUi: IUi
    {
        private String helloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку [pinger -help]";
        private readonly Logger.ILogger _log;
        private readonly IPinger _pinger;
        private readonly IConfigWorker _worker;
        readonly IConsoleUi _outMess;
        readonly IInputsUi _inputs;
        public ConsoleWorkProcessUi()
        {
            IKernel injectKernel = new StandardKernel(new PingerModule.PingerRegistrationModules());
            _log = injectKernel.Get<Logger.ILogger>();
            _pinger = injectKernel.Get<IPinger>();
            _worker = injectKernel.Get<IConfigWorker>();
            _outMess = injectKernel.Get<IConsoleUi>();
            _inputs = injectKernel.Get<IInputsUi>();
            SetUiSettings();
        }

        private String OptionsToWork(String[] keyPress)
        {
            string command = "";
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
                        _outMess.ShowMessage("значение при добавлении не может быть пустым, " +
                                             "\nвызовите справку или ввидите корректное значение для добавления хоста");
                    else
                    {
                        command = _inputs.VerifyString(command, "[", "]");
                        _outMess.ShowMessage(_worker.SaveInConfig(command.Split(' '))
                            ? "\nВведённые данные добавлены успешно! Введите следующую команду:"
                            : "\nОшибка при вводе данных! Введите следующую команду:");
                    }
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Quit:
                    _outMess.ShowMessage("Приложение завершает свою работу");
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
                    _outMess.ShowMessage(
                        "Начинается переодический опрос хостов из файла конфигураций, результат опроса будет записан в логфайл \n\n");
                    ThreadPool.QueueUserWorkItem(StartPingProcess);
                    Thread.Sleep(200);
                    _outMess.ShowMessage("введите команду для выхода [pinger -quit]");
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Show:
                {
                    if (_worker.GetFromConfig().Count > 0)
                    {
                        foreach (KeyValuePair<int, Pinger.PingerModule.Pinger> value in _worker.GetFromConfig())
                        {
                            _outMess.ShowMessage(
                                $"[{value.Key}] Host: {value.Value.Protocol.Host} Interval: {value.Value.Interval} Protocol: {value.Value.Protocol.ProtocolName}");
                        }
                    }
                    else
                        _outMess.ShowMessage("В конфигурационном файле отстувует список хостов, добавьте хост");
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Help:
                {
                    _outMess.ShowMessage(KeyOptions.GetHelpOptions().Aggregate((a, b) => $"{a}\n{b}") + "\n");
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Remove:
                {
                    command = _inputs.VerifyString(command, "[", "]");
                    int index;
                    if (Int32.TryParse(command, out index))
                        _worker.RemoveFromConfig(index);
                    else
                        _outMess.ShowMessage("Вы ввели неверный параметр, пожалуйста пробуйте ввести заново");
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                default:
                    Console.WriteLine(
                        "Неизвестная опция! Повторите ввод или наберите [pinger -help] для вызова справки \n\n");
                    return OptionsToWork(_inputs.ValuesFromUi());
            }
        }

        private void StartPingProcess(object state)
        {
            _pinger.StartWork(_log);
        }

        public void SetUiSettings()
        {
            Console.SetWindowSize((int) (Console.LargestWindowWidth * 0.28),
                (int) (Console.LargestWindowHeight * 0.28));
            Console.SetBufferSize((int) (Console.LargestWindowWidth * 0.28),
                (int) (Console.LargestWindowHeight * 0.28));
        }

        public void RunGui()
        {
            string slash = "";
            for (int x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            slash = slash + "\n" + OptionsToWork("pinger start".Split(' ')) + "\n\n" + slash;
            _outMess.ShowMessage(slash);
            OptionsToWork(_inputs.ValuesFromUi());
        }
    }
    public struct KeyOptions
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
