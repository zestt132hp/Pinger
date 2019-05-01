using System;
using Pinger.Logger;

namespace Pinger.PingerModule
{
    public interface IPingerProcessor
    {
        void Ping(Int32 index, ILogger logger);
        void Ping(ILogger logger);
        void StopPing();
    }
}
