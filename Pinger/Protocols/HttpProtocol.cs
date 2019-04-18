using System;
using System.Net;
using System.Text.RegularExpressions;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.Modules
{
    class HttpProtocol : IProtocol
    {
        private readonly Regex _regex = new Regex(string.Format("^(http|https)://"));
        private string _host;
        private static HttpStatusCode code = HttpStatusCode.OK;
        public Int16 StatusCode { get; set; } = (Int16)code;

        public HttpProtocol(string hostname)
        {
            TryHost(hostname);
        }
        private void TryHost(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
                throw new ArgumentNullException(nameof(hostName));
            if (!_regex.IsMatch(hostName))
                _host = "http://" + hostName; //присваиваем по умолчанию 
            _host = hostName;
        }
        public string Host {
            get { return _host; }
            set
            {
                TryHost(value);
            } 
        }

        //public Double Interval { get; set; }

        public RequestStatus SendRequest(ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}
