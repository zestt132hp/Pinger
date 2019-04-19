using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Modules
{
    internal class IcmpProtocol:IProtocol
    {
        public string Host { get; set; }
        private static String _message = "data";
        public string Message { get; set; } = _message;
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
                var dataBytes = Encoding.ASCII.GetBytes(Message);
                if (ipAddress != null)
                {
                    reply = pingSender.Send(ipAddress, timeout, dataBytes, options);
                }
                if (reply != null)
                {
                    var bytes = reply.Buffer;
                    var responseData = Encoding.ASCII.GetString(dataBytes, 0, bytes.Length);
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
                logger.Write(new Exception("Исключение при отправке запроса. \nТекст ошибки: " + ex));
                Message = $"Ошибка при получении данных";
                return new RequestStatus(false);
            }
        }
    }
}
