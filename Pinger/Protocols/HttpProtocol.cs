﻿using System;
using System.Net;
using System.Text.RegularExpressions;
using Pinger.Logger;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class HttpProtocol :IProtocol
    {
        private readonly Regex _regex = new Regex(string.Format("^(http|https)://"));
        private string _host;
        private static HttpStatusCode _code = HttpStatusCode.OK;
        public Int16 StatusCode { get; private set; } = (Int16)_code;
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
            if (_regex.IsMatch(hostName))
                _host = hostName;
            _host = "http://" + hostName;
        }
        public string Host {
            get => _host;
            set => TryHost(value);
        }
        public RequestStatus SendRequest(ILogger logger) 
        {
            try
            {
                using (HttpWebResponse resp =(HttpWebResponse) WebRequest.Create(new Uri(Host)).GetResponse())
                {
                    Message = $"Получен ответ: {resp.StatusDescription}";
                    return new RequestStatus(resp.StatusCode == _code);
                }
            }
            catch (Exception e)
            {
                logger.Write(e);
                return new RequestStatus(false);
            }
        }
    }
}
