using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace Pinger.ConfigurationModule
{
    class ConfigurationReader: IConfigReader
    {
        private static XElement rootNode;
        private static String configFileName { get; set; }
        private static List<PingProtocol>protocolsList= new List<PingProtocol>();
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
        private static List<PingProtocol> GetProtocolsFromConfig(CustomConfigAttribute attribute)
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
                            });
            foreach (var item in items)
                protocolsList.Add(Protocols.ProtocolCreator.CreateProtocol(item));
            return protocolsList;
        }
        public void SaveInConfig(Enum enumValue, string entryValue)
        {
            if (entryValue == null)
            {
                entryValue = String.Empty;
            }

            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            SaveInConfig(customAttribute, entryValue);
        }

        public Boolean AddInConfig(params string[] values)
        {
            if (!values.Any())
                return false;
            else
            {
                CustomConfigAttribute at = CustomConfigAttribute.CreateConfigAttribute(values);
                SaveInConfig(at, configFileName);
                return true;
            }
        }

        public IList<IProtocol> GetHosts()
        {
            if (!protocolsList.Any())
            {
                GetProtocolsFromConfig(Configuration.RefreshRate);
                GetHosts();
            }
            return protocolsList.Select(x => x.ProtocolInfo).ToList();
        }

        public void RemoveHost()
        {
            throw new NotImplementedException();
        }

        public IList<PingProtocol> GetProtocolsFromConfig(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            return GetProtocolsFromConfig(customAttribute);
        }
        private static void SaveInConfig(CustomConfigAttribute attribute)
        {
            if (rootNode == null)
                return;
            var item = 

            throw new NotImplementedException();
            /*if (rootNode == null)
            {
                return;
            }

           /* var item = from applicationNode in rootNode.Elements("Module")
                       where (String)applicationNode.Attribute("Name") ==
                              attribute.ApplicationName
                       from sectionNode in applicationNode.Elements("Section")
                       where (String)sectionNode.Attribute("Name") ==
                              attribute.SectionName
                       from entryNode in sectionNode.Elements("Entry")
                       where (String)entryNode.Attribute("Name") ==
                              attribute.EntryName
                       select entryNode;

            if (!item.Any())
            {
                return;
            }

            item.First().Value = entryValue;
            SaveConfigFile(configFileName);*/
        }
    }

    interface IConfigReader
    {
        IList<PingProtocol> GetProtocolsFromConfig(Enum enumValue);
        void SaveInConfig(Enum enumValue, String entryValue);
        Boolean AddInConfig(params string[] values);

        IList<IProtocol> GetHosts();
        void RemoveHost();
    }
}
