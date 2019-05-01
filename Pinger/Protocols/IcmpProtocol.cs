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
        public String Host { get;}
        private String _message = "DataTest";
        public String ProtocolName => "Icmp";
        public String Message => _message;
        public IcmpProtocol(String host)
        {
            Host = String.IsNullOrEmpty(host) ? host : "localhost";
        }
        public RequestStatus SendRequest(ILogger logger)
        {
            const Int32 timeout = 120;
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
                    _message = $"Получены данные {responseData}";
                    return new RequestStatus(reply.Status == IPStatus.Success);
                }
                else
                {
                    _message = $"Полученные даннные отсутвуют {nameof(reply)}";
                    return new RequestStatus(false);
                }
            }
            catch (Exception ex)
            {
                logger.Write(ex);
                _message = $"Ошибка при получении данных";
                return new RequestStatus(false);
            }
        }
    }
}
