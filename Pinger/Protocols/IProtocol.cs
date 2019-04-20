using System;
using Pinger.Logger;

namespace Pinger.Protocols
{
    interface IProtocol
    {
        string Host { get; set; }
        string ProtocolName { get; }
        string Message { get; set; }
        RequestStatus SendRequest(ILogger logger);
    }
}
