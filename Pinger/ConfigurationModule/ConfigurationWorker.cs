using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Pinger.Protocols;

namespace Pinger.ConfigurationModule
{
    internal sealed class ConfigurationWorker: IConfigWorker
    {
        #region Private Members & Methods & Property

        private static XElement _rootNode;
        private event EventHandler Refresh;
        private static String ConfigFileName { get; set; }
        private static readonly Dictionary<int, PingerModule.Pinger> ListProtocols = new Dictionary<int, PingerModule.Pinger>();
        private static readonly String DataConfiguration = "DataConfiguration";

        private void ConfigurationReader_Refresh(object sender, EventArgs e)
        {
            GetProtocolsFromConfig(Configuration.RefreshRate);
        }
        private static Dictionary<int, PingerModule.Pinger> GetProtocolsFromConfig(CustomConfigAttribute attribute)
        {
            if(ListProtocols.Any())
                ListProtocols.Clear();
            if (_rootNode == null)
            {
                return null;
            }
            var items =
                _rootNode.Elements(DataConfiguration)
                    .Select(
                        x =>
                            new CustomConfigAttribute()
                            {
                                Host = x.Element(nameof(CustomConfigAttribute.Host))?.Value,
                                Interval = x.Element(nameof(CustomConfigAttribute.Interval))?.Value,
                                Protocol = x.Element(nameof(CustomConfigAttribute.Protocol))?.Value,
                                Port = x.Element(nameof(CustomConfigAttribute.Port))?.Value,
                                HttpCode = x.Element(nameof(CustomConfigAttribute.HttpCode))?.Value
                            }).ToList();

            for (int x = 0; x < items.Count(); x++)
            {
                ListProtocols.Add(x, ProtocolCreator.CreateProtocol(items[x]));
            }
            return ListProtocols;

        }
        private Dictionary<int, PingerModule.Pinger> GetProtocolsFromConfig(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            return GetProtocolsFromConfig(customAttribute);
        }
        private void SaveInConfig(CustomConfigAttribute attribute)
        {
            if (_rootNode == null)
                return;
            XElement dataConfiguration = new XElement(DataConfiguration);
            dataConfiguration.Add(new XElement(nameof(attribute.Host), attribute.Host, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Host) ?? ""))),
                new XElement(nameof(attribute.Interval), attribute.Interval, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Interval) ?? ""))),
                new XElement(nameof(attribute.Protocol), attribute.Protocol, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Protocol) ?? ""))));
            if (attribute.HttpCode != null)
                dataConfiguration.Add(new XElement(nameof(attribute.HttpCode), attribute.HttpCode, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.HttpCode) ?? ""))));
            else
            if (attribute.Port != null)
                dataConfiguration.Add(new XElement(nameof(attribute.Port), attribute.Port, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Port) ?? ""))));
            _rootNode.Add(dataConfiguration);
            _rootNode.Save(ConfigFileName);
            Refresh?.Invoke(this, new EventArgs());
        }

        #endregion



        #region Public Methods

        public ConfigurationWorker(String fileName)
        {
            string uri = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName;
            if (File.Exists(uri))
            {
                _rootNode = XElement.Load(uri);
                ConfigFileName = fileName;
                Refresh += ConfigurationReader_Refresh;
            }
            else
                CreateConfig();
        }



        public Boolean SaveInConfig(params string[] values)
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

        public Dictionary<int, PingerModule.Pinger> GetFromConfig()
        {
            if (!ListProtocols.Any())
            {
                Refresh?.Invoke(this, new EventArgs());
                if (!ListProtocols.Any())
                    return ListProtocols;
                GetFromConfig();
            }
            return ListProtocols;
        }

        public Boolean RemoveFromConfig(Int32 index)
        {
            if (!ListProtocols.Any())
            {
                Refresh?.Invoke(this, new EventArgs());
                if (!ListProtocols.Any())
                    return false;
                else
                    RemoveFromConfig(index);
            }
            if (ListProtocols.ContainsKey(index))
            {
                Pinger.PingerModule.Pinger pinMod = ListProtocols[index];
                _rootNode.Elements(DataConfiguration)
                    .Select(x => x).ToArray()[index].Remove();
                _rootNode.Save(ConfigFileName);
                Refresh?.Invoke(this, new EventArgs());
                return true;
            }
            else
                return false;
        }

        public void CreateConfig()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigFileName))
            {
                using (FileStream fs = new FileStream(ConfigFileName, FileMode.Create))
                {
                    _rootNode = new XElement("configuration");
                    _rootNode.Save(fs);
                }
            }
        }
    }
    #endregion



    internal interface IConfigWorker
    {
        Boolean SaveInConfig(params string[] values);

        Dictionary<int, PingerModule.Pinger> GetFromConfig();
        Boolean RemoveFromConfig(Int32 index);
        void CreateConfig();
    }
}



