using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Xml.Linq;
using Castle.Core.Internal;
using Ninject.Infrastructure.Language;
using Pinger.PingerModule;
using Pinger.Protocols;
using Pinger.ConfigurationModule;

namespace Pinger.ConfigurationModule
{
    internal class ConfigurationReader: IConfigReader
    {
        private static XElement rootNode;
        private static String configFileName { get; set; }
        private static List<PingerModule.Pinger> protocolsList = new List<PingerModule.Pinger>();
        private static Hashtable _listProtocols = new Hashtable();
        public ConfigurationReader(String fileName)
        {
            string uri = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName;
            if (File.Exists(uri))
            {
                rootNode = XElement.Load(uri);
                configFileName = fileName;
            }
        }

        private static void SaveConfigFile(String fileName)
        {
            rootNode.Save(fileName);
        }

        private static Hashtable GetProtocolsFromConfig(CustomConfigAttribute attribute)
        {
            if (rootNode == null)
            {
                return null;
            }
            var items =
                rootNode.Elements("DataConfiguration")
                    .Select(
                        x =>
                            new CustomConfigAttribute()
                            {
                                Host = x.Element(nameof(CustomConfigAttribute.Host))?.Value,
                                Interval = x.Element(nameof(CustomConfigAttribute.Interval))?.Value,
                                Protocol = x.Element(nameof(CustomConfigAttribute.Protocol))?.Value,
                                Port = x.Element(nameof(CustomConfigAttribute.Port))?.Value,
                                HttpCode = x.Element(nameof(CustomConfigAttribute.HttpCode))?.Value
                            }).ToArray();
            for (int x = 0; x < items.Count(); x++)
            {
                _listProtocols.Add(x, items[x]);
            }
            return _listProtocols;
        }

        public Boolean AddHostInConfig(params string[] values)
        {
            if (!values.Any())
                return false;
            else
            {
                CustomConfigAttribute at = CustomConfigAttribute.CreateConfigAttribute(values);
                SaveInConfig(at);
                return true;
            }
        }

        public Hashtable GetHosts()
        {
            if (!protocolsList.Any())
            {
                GetProtocolsFromConfig(Configuration.RefreshRate);
                GetHosts();
            }

            return _listProtocols;
        }

        public void RemoveHost()
        {
            throw new NotImplementedException();
        }

        public Hashtable GetProtocolsFromConfig(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof (CustomConfigAttribute)) as CustomConfigAttribute;
            return GetProtocolsFromConfig(customAttribute);
        }

        public void SaveInConfig(CustomConfigAttribute attribute)
        {
            if (rootNode == null)
                return;
            XElement dataConfiguration = new XElement("DataConfiguration");
            dataConfiguration.Add(new XElement(nameof(attribute.Host), attribute.Host, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Host) ?? ""))),
                new XElement(nameof(attribute.Interval), attribute.Interval, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Interval) ?? ""))),
                new XElement(nameof(attribute.Protocol), attribute.Protocol, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Protocol) ?? ""))));
            if(attribute.HttpCode!=null)
                dataConfiguration.Add(new XElement(nameof(attribute.HttpCode), new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.HttpCode) ?? ""))));
            else
            if(attribute.Port!=null)
                dataConfiguration.Add(new XElement(nameof(attribute.Port), new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Port)??""))));
            rootNode.Add(dataConfiguration);
            rootNode.Save(configFileName);
        }
    }
    internal interface IConfigReader
    {
        Hashtable GetProtocolsFromConfig(Enum enumValue);
        void SaveInConfig(CustomConfigAttribute attr);
        Boolean AddHostInConfig(params string[] values);

        Hashtable GetHosts();
        void RemoveHost();
    }
}



