using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public class IcmpProtocol:IProtocol
    {
        public string Host { get; set; }
        private static String _message = "DataTest";
        public string ProtocolName => "Icmp";
        public string Message { get; set; } = _message;
        public IcmpProtocol(string host)
        {
            if (string.IsNullOrEmpty(host))
                Host = host;
            else
                Host = "localhost";
        }
        public RequestStatus SendRequest(ILogger logger)
        {
            const int timeout = 120;
            var reply = default(PingReply);

            try
            {
                var pingSender = new Ping();
                var options = new PingOptions();
                var ipHostEntry = Dns.GetHostEntry(Host);
                var ipAddress = ipHostEntry.AddressList.FirstOrDefault();
                options.DontFragment = true;
                var dataBytes = Encoding.UTF8.GetBytes(_message);
                if (ipAddress != null)
                {
                    reply = pingSender.Send(ipAddress, timeout, dataBytes, options);
                }
                if (reply != null)
                {
                    var bytes = reply.Buffer;
                    var responseData = Encoding.UTF8.GetString(dataBytes, 0, bytes.Length);
                    Message = $"Получены данные {responseData}";
                    return new RequestStatus(reply.Status == IPStatus.Success);
                }
                else
                {
                    Message = $"Полученные даннные отсутвуют {nameof(reply)}";
                    return new RequestStatus(false);
                }
            }
            catch (Exception ex)
            {
                logger.Write(ex);
                Message = $"Ошибка при получении данных";
                return new RequestStatus(false);
            }
        }
    }
}
