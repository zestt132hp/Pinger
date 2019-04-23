using Pinger.Logger;

namespace Pinger.PingerModule
{
    interface IPinger
    {
        void StartWork(ILogger logger);
        void StopWork();
    }
}
