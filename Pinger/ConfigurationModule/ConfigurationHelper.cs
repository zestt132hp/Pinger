using System;
using System.Linq;

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
    public class CustomConfigAttribute: Attribute
    {
        public String Host { get; set; }
        public String Interval { get; set; }
        public String Protocol { get; set; }
        public String HttpCode { get; set; }
        public String Port { get; set; }

        public static CustomConfigAttribute CreateConfigAttribute(params String[] values)
        {
            CustomConfigAttribute conf = new CustomConfigAttribute();
            var properties = typeof(CustomConfigAttribute).GetProperties();
            for (Int32 x = 0; x < values.Length; x++)
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

        public static CustomConfigAttribute CreateConfigAttribute(Array values)
        {
            CustomConfigAttribute confAttr = new CustomConfigAttribute();
            var properties = confAttr.GetType().GetProperties();
            Int32 index = 0;
            foreach (string val in values)
            {
                var prop = confAttr.GetType().GetProperty(properties[index].Name);
                prop.SetValue(confAttr, val);
                index++;
            }
            return confAttr;
        }

        public static String GetValueAttribute(String nameProperty)
        {
            var memInfo = typeof(Configuration).GetMember(Configuration.RefreshRate.ToString()).First();
            var cusAttrobutes = memInfo.CustomAttributes.Select(x => x.NamedArguments).First();
            for (int x = 0; x < cusAttrobutes.Count(); x++)
            {
                if (cusAttrobutes[x].MemberName == nameProperty)
                    return cusAttrobutes[x].TypedValue.Value.ToString();
            }
            return null;
        }
    }
}
