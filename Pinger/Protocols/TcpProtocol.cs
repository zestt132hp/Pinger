﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public class TcpProtocol: IProtocol
    {
        private static readonly string regExpressionForChekIp = @"^(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])(\.(25[0-5]|2[0-4][0-9]|[0-1][0-9]{2}|[0-9]{2}|[0-9])){3}";
        private static readonly string regExForPort = "|([:][0-9]{2,5})";
        private Regex reg = new Regex(regExpressionForChekIp);
        private Int32 _port;
        private static String message = "DataTest";
        private String _host;

        public int Port
        {
            get { return _port; }
            set {
                _port = value < 80 ? 80 : value;
            }
        }

        public string ProtocolName =>"Tcp/Ip";
        public string Message { get; set; } = message;
        public string Host {
            get { return _host; }
            set { TryHost(value);} 
        }

        public TcpProtocol(String hostName)
        {
            TryHost(hostName);
        }

        private void TryHost(string hostName)
        {
            if (hostName.Contains(":")&& new Regex(regExpressionForChekIp + regExForPort).IsMatch(hostName))
            {
                string[] array = hostName.Split(':');
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
                    Message = responseData;
                    if (readBytes > 0)
                    {
                        return new RequestStatus(isSuccess:true);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(new Exception($"{Host}:{Port} || {e.Message}"));
                Message = "Fail";
            }
            return new RequestStatus(isSuccess: false) ;
        }
    }
}
