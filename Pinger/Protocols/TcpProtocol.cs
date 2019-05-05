using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public class TcpProtocol: IProtocol
    {
        private static readonly String regExpressionForChekIp = @"^(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])(\.(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])){3}";
        private static readonly String regExForPort = "|([:][0-9]{2,5})";
        private Regex reg = new Regex(regExpressionForChekIp);
        private Int32 _port;
        private String _message = "DataTest";
        private String _host;

        public Int32 Port
        {
            get => _port;
            set => _port = value < 80 ? 80 : value;
        }

        public String ProtocolName =>"Tcp/Ip";

        public String Message => _message;

        public String Host => _host;

        public TcpProtocol(String hostName)
        {
            TryHost(hostName);
        }

        private void TryHost(String hostName)
        {
            if (hostName.Contains(":")&& new Regex(regExpressionForChekIp + regExForPort).IsMatch(hostName))
            {
                String[] array = hostName.Split(':');
                _host = array[0];
                Int32.TryParse(array[1], out _port);
            }
            else
            {
                if (!reg.IsMatch(hostName))
                    throw new FormatException("Некорретно введён адрес хоста");
                _host = hostName;
            }
        }

        public RequestStatus SendRequest<T>(ILogger<Exception> logger)
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
                    _message = responseData;
                    if (readBytes > 0)
                    {
                        return new RequestStatus(isSuccess:true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(new Exception($"{Host}:{Port} || {e.Message}"));
                _message = "Fail";
            }
            return new RequestStatus(isSuccess: false) ;
        }
    }
}
