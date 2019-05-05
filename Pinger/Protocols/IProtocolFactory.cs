using System;
using System.Net;

namespace Pinger.Protocols
{
    public interface IProtocolFactory
    {
        HttpProtocol CreateHttpProtocol(String hostName, HttpStatusCode status);
        HttpProtocol CreateHttpProtocol(String hostName);
        TcpProtocol CreaTcpProtocol(String hostName);
        IcmpProtocol CreateIcmpProtocol(String hostName);
    }
}
