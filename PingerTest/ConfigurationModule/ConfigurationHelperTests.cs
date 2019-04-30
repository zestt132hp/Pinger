using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.ConfigurationModule;

namespace PingerTest.ConfigurationModule
{
    [TestClass]
    public class ConfigurationHelperTests
    {
        [TestMethod]
        public void CreateConfigAttrWorkTest()
        {
            //Arrange
            var attribute1 = new CustomConfigAttribute();
            CustomConfigAttribute attribute2;
            string host = "http://www.yandex.ru",
                Interval = "5",
                Protocol = "http/https",
                HttpCode = "123",
                Port = "";

            //Act
            attribute1.Host = host;
            attribute1.HttpCode = HttpCode;
            attribute1.Protocol = Protocol;
            attribute1.Interval = Interval;
            attribute1.Port = Port;
            attribute2 = CustomConfigAttribute.CreateConfigAttribute(host, Interval, Protocol, HttpCode, Port);

            //Assert
            Assert.AreSame(attribute1.Host, attribute2.Host);
            Assert.AreSame(attribute1.HttpCode, attribute1.HttpCode);
            Assert.AreSame(attribute1.Protocol, attribute2.Protocol);
            Assert.AreSame(attribute1.Interval, attribute1.Interval);
            Assert.AreNotSame(attribute1.Port, attribute2.Port);
            Assert.AreNotEqual(attribute1, attribute2);

        }

        [TestMethod]
        public void CreateAttrFromArrayType()
        {
            //Arrange
            var attribute1 = new CustomConfigAttribute();
            CustomConfigAttribute attribute2;
            string host = "http://www.yandex.ru",
                Interval = "5",
                Protocol = "http/https",
                HttpCode = "123",
                Port = "";
            Array array = new[] {host, Interval, Protocol, HttpCode, Port};

            //Act
            attribute1.Host = host;
            attribute1.HttpCode = HttpCode;
            attribute1.Protocol = Protocol;
            attribute1.Interval = Interval;
            attribute1.Port = Port;
            attribute2 = CustomConfigAttribute.CreateConfigAttribute(array);

            //Assert
            Assert.AreEqual(attribute1.Host, attribute2.Host);
            Assert.AreEqual(attribute1.HttpCode, attribute2.HttpCode);
            Assert.AreEqual(attribute1.Interval, attribute2.Interval);
            Assert.AreEqual(attribute1.Port, attribute2.Port);
            Assert.AreEqual(attribute1, attribute2);
        }

        [TestMethod]
        public void GetValueTest()
        {
            //Arrange
            var property = new CustomConfigAttribute().GetType().GetProperties();
            string[] array = new string[property.Length - 1];

            //Act
            for (int x = 0; x < property.Length - 1; x++)
            {
                array[x] = CustomConfigAttribute.GetValueAttribute(property[x].Name);
            }

            //Assert
            Assert.IsNotNull(array);
            foreach (string t in array)
            {
                Assert.IsNotNull(t);
            }
        }
    }
}
