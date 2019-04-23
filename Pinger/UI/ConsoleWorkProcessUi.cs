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
        private readonly Logger.ILogger _log;
        private readonly IPinger _pinger;
        private readonly IConfigWorker _worker;
        readonly IConsoleOutputUi _outMess;
        readonly IInputsUi _inputs;
        private ConsoleKeyInfo cki;
        public ConsoleWorkProcessUi()
        {
            IKernel injectKernel = new StandardKernel(new PingerModule.PingerRegistrationModules());
            _log = injectKernel.Get<Logger.ILogger>();
            _pinger = injectKernel.Get<IPinger>();
            _worker = injectKernel.Get<IConfigWorker>();
            _outMess = injectKernel.Get<IConsoleOutputUi>();
            _inputs = injectKernel.Get<IInputsUi>();
            SetUiSettings();
        }

        private String OptionsToWork(String[] keyPress)
        {
            if (keyPress == null)
                return null;
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
                    {
                        _outMess.PrintMessage("значение при добавлении не может быть пустым, ");
                        _outMess.PrintMessage("вызовите справку или ввидите корректное значение для добавления хоста", "\n", false);
                    }

                    else
                    {
                        try
                        {
                            if (_inputs.VerifyString(ref command, "[", "]"))
                                _outMess.PrintMessage(
                                    _worker.SaveInConfig(command.Split(' '))
                                        ? "Введённые данные добавлены успешно! Введите следующую команду:"
                                        : "Ошибка при вводе данных! Введите следующую команду:", "\n", false);
                            else
                                _outMess.PrintMessage("Ошибка при вводе данных! Введите следующую команду:", "\n",false);
                        }
                        catch (NotImplementedException e)
                        {
                            _outMess.PrintMessage(e.Message);
                        }
                    }

                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Quit:
                    _outMess.PrintMessage("Приложение завершает свою работу");
                    Thread.Sleep(1000);
                    try
                    {
                        Thread.CurrentThread.Abort();
                    }
                    catch (ThreadAbortException)
                    {
                        Environment.Exit(0);
                    }

                    return null;
                case KeyOptions.Start:
                    return KeyOptions.HelloMessage;
                case KeyOptions.Ping:
                {
                    _outMess.PrintMessage("Начинается переодический опрос хостов из файла конфигураций, результат опроса будет записан в логфайл");
                    _outMess.PrintMessage("Для отмены нажмите Any Key или для выхода Ctrl+C", "\n", true);
                    Thread.Sleep(1000);
                    ThreadPool.QueueUserWorkItem(StartPingProcess);
                    Console.CancelKeyPress += KeyPress;
                    Thread.Sleep(200);
                    cki = Console.ReadKey(false);
                    if (cki != null)
                    {
                        _pinger.StopWork();
                        Thread.CurrentThread.Join(2000);
                    }

                    Thread.Sleep(3005);
                    _outMess.PrintMessage("Процесс остановлен введите команду...", "\n", false);
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Show:
                {
                    try
                    {
                        if (_worker.GetFromConfig().Count > 0)
                        {
                            foreach (KeyValuePair<int, Pinger.PingerModule.Pinger> value in _worker.GetFromConfig())
                            {
                                _outMess.PrintMessage(
                                    $"[{value.Key}] Host: {value.Value.Protocol.Host} Interval: {value.Value.Interval} Protocol: {value.Value.Protocol.ProtocolName}");
                            }
                        }
                        else
                            _outMess.PrintMessage("В конфигурационном файле отстувует список хостов, добавьте хост");
                    }
                    catch (NotImplementedException e)
                    {
                        _outMess.PrintMessage(e.Message);
                    }
                    finally
                    {
                        OptionsToWork(_inputs.ValuesFromUi());
                    }

                    return null;
                }
                case KeyOptions.Help:
                {
                    _outMess.PrintMessage(KeyOptions.GetHelpOptions().Aggregate((a, b) => $"{a}\n{b}"));
                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Remove:
                {
                    if (!_inputs.VerifyString(ref command, "[", "]"))
                        _outMess.PrintMessage("Введенные данные не верны");
                    if (Int32.TryParse(command, out var index))
                    {
                        _outMess.PrintMessage(_worker.RemoveFromConfig(index) ? "Протокол удалён" : "Ошибка удаления");
                    }
                    else
                        _outMess.PrintMessage("Вы ввели неверный параметр, пожалуйста пробуйте ввести заново");

                    OptionsToWork(_inputs.ValuesFromUi());
                    return null;
                }
                case KeyOptions.Stop:
                    _pinger.StopWork();
                    _outMess.PrintMessage("Введите процесс опроса остановлен, введите команду:");
                    OptionsToWork((_inputs.ValuesFromUi()));
                    return null;
                default:
                    _outMess.PrintMessage(
                        "Неизвестная опция! Повторите ввод или наберите [pinger -help] для вызова справки" , "\n", true);
                    return OptionsToWork(_inputs.ValuesFromUi());
            }
        }

        private void KeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _outMess.PrintMessage("Остановка рабчего процесса");
            _pinger.StopWork();
            e.Cancel = true;
            _outMess.PrintMessage("Все процессы остановлены программа будет завершена через 5 сек.");
            Thread.Sleep(500);
            Environment.Exit(0);
        }

        private void StartPingProcess(object state)
        {
            _pinger.StartWork(_log);
        }

        public void SetUiSettings()
        {
            Console.SetWindowSize((int) (Console.LargestWindowWidth * 0.28),
                (int) (Console.LargestWindowHeight * 0.28));
            Console.SetBufferSize((int) (Console.LargestWindowWidth * 0.28), Console.BufferHeight);
        }

        public void RunGui()
        {
            string slash = "";
            for (int x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            slash = slash + "\n" + OptionsToWork("pinger start".Split(' ')) + "\n\n" + slash;
            _outMess.PrintMessage(slash);
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
        public const String Stop = "s";

        public static string[] GetHelpOptions()
        {
            return new[] {
                Add + " - позволяет добавить имя хоста в конфигурационный файл \nПример для ICMP протокола " + Add + " [wwww.yandex.ru 5 ICMP] \n" +
                "Пример для Http/Https протокола: [www.yandex.ru 16 http 200]\n - последняя цифра это статус-код \n" +
                "Пример для tcp/ip протокола: [10.200.224.94:50 15 tcp/ip]\n - через двоеточие указывается порт",
                Quit + " - Завершает работу приложения",
                Remove + " [№]  - удаляет хост с указанным номером",
                Show  + " - Отображает сохраненные хосты из файла конфигураций",
                Ping + " - Запускает опрос указанных хостов в конфиг-файле"
            };
        }
        public const String HelloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку [pinger -help]";
    }

   
}
