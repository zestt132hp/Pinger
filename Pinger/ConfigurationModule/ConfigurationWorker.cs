using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Pinger.Protocols;

namespace Pinger.ConfigurationModule
{
    public sealed class ConfigurationWorker: IConfigWorker
    {
        #region Private Members & Methods & Property

        private XElement _rootNode;
        private event EventHandler Refresh;
        private String ConfigFileName { get; set; }
        private readonly Dictionary<Int32, PingerModule.IPinger> _listProtocols = new Dictionary<Int32, PingerModule.IPinger>();
        private readonly String DataConfiguration = "DataConfiguration";
        private readonly IProtocolFactory _factory;
        private ProtocolCreator _creator;

        private void ConfigurationReader_Refresh(object sender, EventArgs e)
        {
            GetProtocolsFromConfig(Configuration.RefreshRate);
        }
        private Dictionary<Int32, PingerModule.IPinger> GetProtocolsFromConfig()
        {
            if(_listProtocols.Any())
                _listProtocols.Clear();
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
            for (Int32 x = 0; x < items.Count(); x++)
            {
                _listProtocols.Add(x, _creator.CreateProtocol(items[x]));
            }
            return _listProtocols;

        }
        private Dictionary<Int32, PingerModule.IPinger> GetProtocolsFromConfig(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo info = type.GetField(enumValue.ToString());
            var customAttribute = Attribute.GetCustomAttribute(info,
                typeof(CustomConfigAttribute)) as CustomConfigAttribute;
            return GetProtocolsFromConfig();
        }
        private void SaveInConfig(CustomConfigAttribute attribute)
        {
            if (_rootNode == null)
                return;
            XElement dataConfiguration = new XElement(DataConfiguration);
            dataConfiguration.Add(new XElement(nameof(attribute.Host), attribute.Host, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Host)))),
                new XElement(nameof(attribute.Interval), attribute.Interval, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Interval)))),
                new XElement(nameof(attribute.Protocol), attribute.Protocol, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Protocol)))));
            if (attribute.HttpCode != null)
                dataConfiguration.Add(new XElement(nameof(attribute.HttpCode), attribute.HttpCode, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.HttpCode)))));
            else
            if (attribute.Port != null)
                dataConfiguration.Add(new XElement(nameof(attribute.Port), attribute.Port, new XAttribute("Name", CustomConfigAttribute.GetValueAttribute(nameof(attribute.Port)))));
            _rootNode.Add(dataConfiguration);
            _rootNode.Save(ConfigFileName);
            Refresh?.Invoke(this, new EventArgs());
        }

        #endregion

        public ConfigurationWorker(String fileName, IProtocolFactory factory, ProtocolCreator creator)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            _factory = factory ?? throw new NullReferenceException(nameof(ConfigurationWorker));
            _creator = creator ?? throw new NullReferenceException(
                           nameof(ConfigurationWorker) + nameof(ProtocolCreator));
            ConfigFileName = fileName;
            String uri = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName;
            if (File.Exists(uri))
            {
                _rootNode = XElement.Load(uri);
                Refresh += ConfigurationReader_Refresh;
            }
            else
                CreateConfig();
        }

        #region Public Methods

        public Boolean SaveInConfig(params String[] values)
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

        public Dictionary<Int32, PingerModule.IPinger> GetFromConfig()
        {
            if (!_listProtocols.Any())
            {
                Refresh?.Invoke(this, new EventArgs());
                if (!_listProtocols.Any())
                    return _listProtocols;
                GetFromConfig();
            }
            return _listProtocols;
        }

        public Boolean RemoveFromConfig(Int32 index)
        {
            if (!_listProtocols.Any())
            {
                Refresh?.Invoke(this, new EventArgs());
                if (!_listProtocols.Any())
                    return false;
                else
                    RemoveFromConfig(index);
            }
            if (_listProtocols.ContainsKey(index))
            {
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



   
}



