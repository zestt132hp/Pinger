using Pinger.Logger;

namespace Pinger.Protocols
{
    public interface IProtocol
    {
        string Host { get; set; }
        string ProtocolName { get; }
        string Message { get; set; }
        RequestStatus SendRequest(ILogger logger);
    }
}
