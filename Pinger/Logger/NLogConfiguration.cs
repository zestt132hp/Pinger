using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Pinger.Logger
{
    public sealed class NlogConfiguration: IConfigurationNlog
    {
        public LoggingConfiguration GetLogConfiguration(String logName)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget(logName);
            FileTarget erorrTarget = new FileTarget(logName+"_errors");
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            config.AddTarget("file", fileTarget);
            config.AddTarget("file", erorrTarget);
            consoleTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            fileTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            erorrTarget.Layout = "${date:format=HH\\:mm\\:ss}| ${message}";
            fileTarget.FileName = "${basedir}/logs/" + $"{logName}.txt";
            erorrTarget.FileName = "${basedir}/logs/" + $"{logName}_errors.txt";
            LoggingRule rule = new LoggingRule("*", LogLevel.Info, consoleTarget);
            config.LoggingRules.Add(rule);
            LoggingRule rule2 = new LoggingRule("*", LogLevel.Error, erorrTarget);
            LoggingRule rule3 =new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
            config.LoggingRules.Add(rule3);
            return config;
        }
    }
}
