using System;
using NLog;

namespace Pinger.Logger
{
    class Logger:ILogger
    {
        private readonly NLog.Logger _logger;
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
                lock (_logger)
                {
                    _logger?.Info(e.Message + "\n" + e.StackTrace);
                }
            }
        }

        private void WriteMessage(String message)
        {

            lock (_logger)
            {
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
