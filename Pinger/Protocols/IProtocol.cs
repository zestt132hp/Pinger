using System;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public interface IProtocol
    {
        String Host { get; }
        String ProtocolName { get; }
        String Message { get; }
        RequestStatus SendRequest(ILogger logger);
    }
}
