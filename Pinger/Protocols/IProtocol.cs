using System;
using Pinger.Logger;

namespace Pinger.Protocols
{
    interface IProtocol
    {
        string Host { get; set; }
        //Double Interval { get; set; }
        RequestStatus SendRequest(ILogger logger);
    }
}
