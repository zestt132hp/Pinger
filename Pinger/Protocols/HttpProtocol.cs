using System;
using System.Net;
using System.Text.RegularExpressions;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public class HttpProtocol :IProtocol
    {
        private readonly Regex _regex = new Regex("^(http|https)://");
        private string _host;
        private HttpStatusCode _code = HttpStatusCode.OK;

        public Int16 StatusCode
        {
            get { return (Int16) _code; }
        }

        public string ProtocolName => "Http/Https";
        public string Message { get; set; }

        public HttpProtocol(string hostname, HttpStatusCode statusCode)
        {
            _code = statusCode;
            TryHost(hostname);
        }

        public HttpProtocol(string hostName)
        {
            TryHost(hostName);
        }

        private void TryHost(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentNullException(nameof(hostName));
            if (!_regex.IsMatch(hostName))
            {
                _host = "http://" + hostName;
                return;
            }
            _host = hostName;
        }
        public string Host {
            get { return _host; }
            set { TryHost(value);}
        }
        public RequestStatus SendRequest(ILogger logger) 
        {
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse) WebRequest.Create(new Uri(Host)).GetResponse())
                {
                    Message = $"Получен ответ: {resp.StatusDescription}";
                    return new RequestStatus(resp.StatusCode == _code);
                }
            }
            catch (WebException e)
            {
                Message = "Неудачный запрос |" + e.Message;
                return new RequestStatus(false);
            }
            catch (Exception e)
            {
                logger.Write(new Exception(Host + "||" + e.Message));
                Message = $"Ошибка при соединении";
                return new RequestStatus(false);
            }
        }
    }
}
