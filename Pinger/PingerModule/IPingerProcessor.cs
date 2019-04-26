using Pinger.Logger;

namespace Pinger.PingerModule
{
    public interface IPingerProcessor
    {
        void StartWork(ILogger logger);
        void StopWork();
    }
}
