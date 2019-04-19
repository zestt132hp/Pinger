using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pinger.ConfigurationModule
{
    public enum Configuration
    {
        [CustomConfig(
            Host = "AddressHost",
            Protocol= "ProtocolType",
            Interval = "TimeInterval",
            HttpCode = "HttpStatusCode",
            Port = "PortTcp")]
        RefreshRate
    }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class CustomConfigAttribute: Attribute
    {
        public object Host { get; set; }
        [DefaultValue(1)]
        public object Interval { get; set; }
        public object Protocol { get; set; }
        [DefaultValue(HttpStatusCode.OK)]
        public object HttpCode { get; set; }
        public object Port { get; set; }
    }
}
