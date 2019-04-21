using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pinger.ConfigurationModule
{
    public enum Configuration
    {
        [CustomConfig(
            Host = "AddressHost",
            Protocol= "ProtocolName",
            Interval = "TimeInterval",
            HttpCode = "HttpStatusCode",
            Port = "PortTcp")]
        RefreshRate
    }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class CustomConfigAttribute: Attribute
    {
        public object Host { get; set; }
        public object Interval { get; set; }
        public object Protocol { get; set; }
        public object HttpCode { get; set; }
        public object Port { get; set; }

        public static CustomConfigAttribute CreateConfigAttribute(params string[] values)
        {
            CustomConfigAttribute conf = new CustomConfigAttribute();
            var properties = typeof(CustomConfigAttribute).GetProperties();
            for (int x = 0; x < values.Length; x++)
            {
                switch (properties[x].Name)
                {
                    case nameof(Host):
                        conf.Host = values[x];
                        break;
                    case nameof(Interval):
                        conf.Interval = values[x];
                        break;
                    case nameof(Protocol):
                        conf.Protocol = values[x];
                        break;
                    case nameof(HttpCode):
                        conf.HttpCode = values[x];
                        break;
                }
            }
            return conf;
        }
    }
}
