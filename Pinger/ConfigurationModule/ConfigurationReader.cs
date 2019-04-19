using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Pinger.PingerModule;

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
        private String FindConfigurationFile(string directoryPath)
        {
            return null;
        }

        private static void SaveConfigFile(String fileName)
        {
            rootNode.Save(fileName);
        }
        private static List<PingProtocol> GetConfigEntry(CustomConfigAttribute attribute)
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
        public void SetConfigEntry(Enum enumValue, string entryValue)
        {
            if (entryValue == null)
            {
                entryValue = String.Empty;
            }

            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            SetConfigEntry(customAttribute, entryValue);
        }

        public IList<PingProtocol> GetConfigEntry(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            return GetConfigEntry(customAttribute);
        }
        private static void SetConfigEntry(CustomConfigAttribute attribute,
                                       String entryValue)
        {
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
        IList<PingProtocol> GetConfigEntry(Enum enumValue);
        void SetConfigEntry(Enum enumValue, String entryValue);
    }
}
