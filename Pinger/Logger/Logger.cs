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

        public Logger(string logName)
        {
            var name = string.IsNullOrEmpty(logName) ? DateTime.Now.ToShortDateString() : logName;
            _logger = LogManager.GetCurrentClassLogger();
            var configuration = PingerRegistrationModules.GetKernel().Get<IConfigurationNlog>();
            LogManager.Configuration = configuration.GetLogConfiguration(name);
            _output = PingerRegistrationModules.GetKernel().Get<IConsoleOutputUi>();
        }

        public void Write<T>(T message)
        {
            try
            {
                Exception e = message as Exception;
                if (e != null)
                {
                    WriteException(e);
                    return;
                }
                string s = message as String;
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

            lock (_logger)
            {
                _logger.Info(message);
                _logger.Debug(message);
            }
        }

        private void WriteException(Exception message)
        {
            lock (_logger)
            {
                _logger.Error(message);
            }
        }
    }
}
