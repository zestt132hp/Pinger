using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ninject;
using Pinger.ConfigurationModule;
using Pinger.PingerModule;

namespace Pinger.UI
{
    public sealed class ConsoleWorkProcessUi: IUi
    {
        private readonly Logger.ILogger _log;
        private readonly IPingerProcessor _pinger;
        private readonly IConfigWorker _worker;
        readonly IConsoleOutputUi _outMess;
        readonly IInputsUi _inputs;
        public IKernel InjectKernel;
        private ConsoleKeyInfo _cki;

        public ConsoleWorkProcessUi()
        {
            InjectKernel = PingerRegistrationModules.GetKernel();
            _log = InjectKernel.Get<Logger.ILogger>();
            _pinger = InjectKernel.Get<IPingerProcessor>();
            _worker = InjectKernel.Get<IConfigWorker>();
            _outMess = InjectKernel.Get<IConsoleOutputUi>();
            _inputs = InjectKernel.Get<IInputsUi>();
            SetUiSettings();
        }

        public void OptionsToWork(String[] keyPress)
        {
            if (keyPress == null)
                throw new ArgumentNullException("keyPress");
            String command = "";
            String option;
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
                    if (!_inputs.VerifyString(ref command))
                        _outMess.PrintMessage("значение при добавлении не может быть пустым, ");
                    _outMess.PrintMessage("вызовите справку или ввидите корректное значение для добавления хоста",
                        "\n", false);
                    CommandAdd(command);
                    OptionsToWork(_inputs.ValuesFromUi());
                    break;
                }
                case KeyOptions.Quit:
                {
                    _outMess.PrintMessage("Приложение завершает свою работу");
                    Console.CancelKeyPress -= KeyPress;
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
                }
                case KeyOptions.Start:
                {
                    _outMess.PrintMessage(KeyOptions.HelloMessage);
                    break;
                }
                case KeyOptions.Ping:
                {
                    Console.CancelKeyPress += KeyPress;
                    if (!CommandPing(command))
                        _outMess.PrintMessage("Неверная команда для старта пингования");
                    OptionsToWork(_inputs.ValuesFromUi());
                    break;
                }
                case KeyOptions.Show:
                {
                    try
                    {
                        CommandShow();
                    }
                    catch (NotImplementedException e)
                    {
                        _outMess.PrintMessage(e.Message);
                    }
                    finally
                    {
                        OptionsToWork(_inputs.ValuesFromUi());
                    }
                    break;
                }
                case KeyOptions.Help:
                {
                    _outMess.PrintMessage(KeyOptions.GetHelpOptions().Aggregate((a, b) => $"{a}\n{b}"), "\n", false);
                    OptionsToWork(_inputs.ValuesFromUi());
                    break;
                }
                case KeyOptions.Remove:
                {
                    if (!_inputs.VerifyString(ref command, "[", "]"))
                        _outMess.PrintMessage("Введенные данные не верны");
                    Int32 index;
                    if (Int32.TryParse(command, out index))
                    {
                        _outMess.PrintMessage(_worker.RemoveFromConfig(index) ? "Протокол удалён" : "Ошибка удаления");
                    }
                    else
                        _outMess.PrintMessage("Вы ввели неверный параметр, пожалуйста пробуйте ввести заново");

                    OptionsToWork(_inputs.ValuesFromUi());
                    break;
                }
                default:
                {
                    _outMess.PrintMessage(
                        "Неизвестная опция! Повторите ввод или наберите [pinger -help] для вызова справки", "\n", true);
                    OptionsToWork(_inputs.ValuesFromUi());
                    break;
                }
            }
        }

        private void StopProcess()
        {
            _cki = Console.ReadKey(false);
            if (_cki != null)
            {
                Task task = Task.Run(() => _pinger.StopPing());
                Task.WhenAll(task);
                Thread.Sleep(1000);
                _outMess.PrintMessage("Процесс остановлен введите команду...", "\n", false);
            }
        }

        private void CommandShow()
        {
            if (_worker.GetFromConfig().Count > 0)
            {
                foreach (KeyValuePair<Int32, IPinger> value in _worker.GetFromConfig())
                {
                    _outMess.PrintMessage(
                        $"[{value.Key}] Host: {value.Value.Protocol.Host} Interval: {value.Value.Interval} Protocol: {value.Value.Protocol.ProtocolName}");
                }
                return;
            }
            _outMess.PrintMessage("В конфигурационном файле отстувует список хостов, добавьте хост");
        }

        private void PingMessage()
        {
            _outMess.PrintMessage(
                        "Начинается переодический опрос хостов из файла конфигураций, результат опроса будет записан в логфайл");
            _outMess.PrintMessage("Для отмены нажмите Any Key или для выхода Ctrl+C", "\n", true);
            Thread.Sleep(2500);
        }

        public Boolean CommandPing(String command)
        {
            if (String.IsNullOrEmpty(command))
                return false;
            Int32 tmp;
            if ((_inputs.VerifyString(ref command, "[", "]")) && Int32.TryParse(command, out tmp))
            {
                PingMessage();
                if (_worker.GetFromConfig().ContainsKey(tmp))
                    InjectKernel.Get<IPingerProcessor>().Ping(_log);
                else return false;
                _cki = Console.ReadKey(false);
                if(_cki!=null)
                    _worker.GetFromConfig().Values.ElementAt(tmp).StopWork();
                return true;
            }
            if (command.ToLowerInvariant() == "all")
            {
                PingMessage();
                ThreadPool.QueueUserWorkItem(StartPingProcess);
                Thread.Sleep(3000);
                StopProcess();
                return true;
            }
            _outMess.PrintMessage("Неверная команда для запуска пингования");
            return false;
        }

        private void CommandAdd(String command)
        {
            try
            {
                if (_inputs.VerifyString(ref command, "[", "]"))
                    _outMess.PrintMessage(
                        _worker.SaveInConfig(command.Split(' '))
                            ? "Введённые данные добавлены успешно! Введите следующую команду:"
                            : "Ошибка при вводе данных! Введите следующую команду:", "\n", false);
                else
                    _outMess.PrintMessage("Ошибка при вводе данных! Введите следующую команду:", "\n",
                        false);
            }
            catch (NotImplementedException e)
            {
                _outMess.PrintMessage(e.Message);
            }
        }

        private void KeyPress(Object sender, ConsoleCancelEventArgs e)
        {
            _outMess.PrintMessage("Остановка рабчего процесса");
            _pinger.StopPing();
            e.Cancel = true;
            _outMess.PrintMessage("Все процессы остановлены программа будет завершена через 5 сек.");
            Thread.Sleep(500);
            Environment.Exit(0);
        }

        private void StartPingProcess(Object state)
        {
            _pinger.Ping(_log);
        }

        private void SetUiSettings()
        {
            if (Console.LargestWindowHeight == 0 && Console.LargestWindowWidth == 0)
                return;
            Console.SetWindowSize((Int32) (Console.LargestWindowWidth * 0.35),
                (Int32) (Console.LargestWindowHeight * 0.5));
            Console.SetBufferSize((Int32) (Console.LargestWindowWidth * 0.35), Console.BufferHeight);
        }

        public void RunGui()
        {
            String slash = "";
            for (Int32 x = 0; x < Console.WindowWidth; x++)
                slash += "-";
            _outMess.PrintMessage(slash, "\n", true);
             OptionsToWork("pinger start".Split(' '));
            _outMess.PrintMessage(slash, "\n", true);
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

        public static String[] GetHelpOptions()
        {
            return new[] {
                Ping + " - Запускает опрос указанных хостов в конфиг-файле \n",
                Ping + " [№] - Запускает опрос указанного хоста \nиз списка файла конфигурации\n",
                Add + " - позволяет добавить имя хоста в конфигурационный файл \n  Пример для ICMP протокола: " + Add + " [wwww.yandex.ru 5 ICMP] \n" +
                "  Пример для Http/Https протокола:\n  "+Add +" [www.yandex.ru 16 http 200]- последняя цифра это статус-код \n" +
                "  Пример для tcp/ip протокола:\n  "+Add+" [10.200.224.94:50 15 tcp/ip]- через двоеточие указывается порт \n",
                Show  + " - Отображает сохраненные хосты из файла конфигураций\n",
                Remove + " [№]  - удаляет хост с указанным номером\n",
                Quit + " - Завершает работу приложения\n"
            };
        }
        public const String HelloMessage = "\t\t Добро пожаловать в \"Pinger\"! \n \t введите команду или вызовите справку [pinger -help]";
    }

   
}
