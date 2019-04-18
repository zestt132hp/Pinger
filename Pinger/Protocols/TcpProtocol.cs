using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class TcpProtocol: IProtocol
    {
        private static Int32 port = 80;
        private static String message = "data";
        public int Port { get; set; } = port;
        public string Message { get; set; } = message;
        public string Host { get; set; }
        //public Double Interval { get; set; }

        public RequestStatus SendRequest(ILogger logger)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(hostname: Host, port: Port);
                    var bytes = Encoding.ASCII.GetBytes(Message);
                    var networkStream = client.GetStream();
                    networkStream.Write(bytes, 0, bytes.Length);
                    bytes = new byte[8];
                    var readBytes = networkStream.Read(bytes, 0, bytes.Length);
                    var responseData = Encoding.ASCII.GetString(bytes, 0, networkStream.Read(bytes, 0, readBytes));
                    if (readBytes > 0)
                    {
                        return new RequestStatus(isSuccess:true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(e);
            }
            return new RequestStatus(isSuccess: false);
        }
    }
}
