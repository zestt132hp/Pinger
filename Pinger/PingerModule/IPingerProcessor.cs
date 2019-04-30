using Pinger.Logger;

namespace Pinger.PingerModule
{
    public interface IPingerProcessor
    {
        void Ping(int index, ILogger logger);
        void Ping(ILogger logger);
        void StopPing();
    }
}
