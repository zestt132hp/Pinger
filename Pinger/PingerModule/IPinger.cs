using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public interface IPinger:IPingerProcessor
    {
        IProtocol Protocol { get; }
        int Interval { get; set; }
    }
}
