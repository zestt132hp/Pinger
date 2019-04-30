using Pinger.Logger;

namespace Pinger.Protocols
{
    public interface IProtocol
    {
        string Host { get; }
        string ProtocolName { get; }
        string Message { get; }
        RequestStatus SendRequest(ILogger logger);
    }
}
