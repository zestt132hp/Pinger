using System;
using Ninject;
using NLog;
using Pinger.PingerModule;
using Pinger.UI;

namespace Pinger.Logger
{
    public class Logger:ILogger
    {
        private readonly NLog.Logger _logger;
        private IConsoleOutputUi _output;

        public Logger(String logName)
        {
            var name = String.IsNullOrEmpty(logName) ? DateTime.Now.ToShortDateString() : logName;
            _logger = LogManager.GetCurrentClassLogger();
            var configuration = PingerRegistrationModules.GetKernel().Get<IConfigurationNlog>();
            LogManager.Configuration = configuration.GetLogConfiguration(name);
            _output = PingerRegistrationModules.GetKernel().Get<IConsoleOutputUi>();
        }

        public void Write<T>(T message)
        {
            try
            {
                var e = message as Exception;
                if (e != null)
                {
                    WriteException(e);
                    return;
                }
                var s = message as String;
                if (s != null)
                {
                    WriteMessage(s);
                }
            }
            catch (Exception e)
            {
                _output.PrintMessage(e.Message, "\n", true);
            }
        }

        private void WriteMessage(String message)
        {
            _logger.Info(message);
            _logger.Debug(message);
        }

        private void WriteException(Exception message)
        {
            _logger.Error(message);
        }
    }
}
