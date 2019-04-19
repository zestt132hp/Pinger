using System;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NLog;
using NLog.Fluent;

namespace Pinger.Logger
{
    class Logger:ILogger
    {
        private NLog.Logger _logger;// = LogManager.GetLogger("PingerLog");

        public Logger(string logName)
        {
            IConfigurationNlog config = new NlogConfiguration();
            LogManager.Configuration = config.GetLogConfiguration(logName);
            _logger = LogManager.GetLogger(logName);
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
                    return;
                }
            }
            catch (Exception e)
            {
            }
        }

        private void WriteMessage(String message)
        {

            lock (_logger)
            {
                _logger.Info(message);
            }
        }

        private void WriteException(Exception message)
        {
            lock(_logger)
                _logger.Error(message);
        }
    }
}
