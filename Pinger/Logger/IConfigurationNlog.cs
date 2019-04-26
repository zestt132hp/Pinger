
using NLog.Config;

namespace Pinger.Logger
{
    public interface IConfigurationNlog
    {
        LoggingConfiguration GetLogConfiguration(string logName);
    }
}
