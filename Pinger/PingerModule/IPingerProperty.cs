using System;
using Pinger.Protocols;

namespace Pinger.PingerModule
{
    public interface IPingerProperty
    {
        IProtocol Protocol { get; }
        Int32 Interval { get; }
    }
}
