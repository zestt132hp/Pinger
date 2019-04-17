using System;
using System.ComponentModel;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class TcpProtocol:Protocol, Protocols.IProtocol
    {
        private static Int32 port = 80;
        public int Port { get; set; } = port;
        public string Message { get; set; }

        public TcpProtocol()
        {
        }
        public void SendRequest(ILogger loger)
        {
            throw new NotImplementedException();
        }
    }
}
