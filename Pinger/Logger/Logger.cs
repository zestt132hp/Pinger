using System;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NLog;
namespace Pinger.Logger
{
    class Logger:ILogger
    {
        NLog.Logger _logger = LogManager.GetLogger("PingerLog");

        public void Write<T>(T message)
        {
            throw new NotImplementedException();
        }
    }
}
