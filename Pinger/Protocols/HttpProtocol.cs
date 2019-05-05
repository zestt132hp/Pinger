using System;
using System.Net;
using System.Text.RegularExpressions;
using Pinger.Logger;

namespace Pinger.Protocols
{
    public class HttpProtocol :IProtocol
    {
        private readonly Regex _regex = new Regex("^(http|https)://");
        private String _host;
        private String _message = "dataString";
        private readonly HttpStatusCode _code = HttpStatusCode.OK;

        public Int16 StatusCode => (Int16) _code;

        public String ProtocolName => "Http/Https";
        public String Message => _message;

        public HttpProtocol(String hostname, HttpStatusCode statusCode)
        {
            _code = statusCode;
            TryHost(hostname);
        }

        public HttpProtocol(String hostName)
        {
            TryHost(hostName);
        }

        private void TryHost(String hostName)
        {
            if (String.IsNullOrEmpty(hostName))
                throw new ArgumentNullException(nameof(hostName));
            if (!_regex.IsMatch(hostName))
            {
                _host = "http://" + hostName;
                return;
            }
            _host = hostName;
        }
        public String Host => _host;

        public RequestStatus SendRequest<T>(ILogger<Exception> logger) 
        {
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse) WebRequest.Create(new Uri(Host)).GetResponse())
                {
                    _message = $"Получен ответ: {resp.StatusDescription}";
                    return new RequestStatus(resp.StatusCode == _code);
                }
            }
            catch (WebException e)
            {
                _message = "Неудачный запрос |" + e.Message;
                return new RequestStatus(false);
            }
            catch (Exception e)
            {
                _message = $"Ошибка при соединении";
                logger.Write(e);
                return new RequestStatus(false);
            }
        }
    }
}
