using NLog;
using NLog.Config;
using NLog.Targets;

namespace Pinger.Logger
{
    class NlogConfiguration: IConfigurationNlog
    {
        public LoggingConfiguration GetLogConfiguration(string logName)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";
            fileTarget.FileName = "${basedir}/"+logName + ".txt";
            fileTarget.Layout = "${message}";
            LoggingRule rule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule);
            LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
            return config;
        }
    }
}
