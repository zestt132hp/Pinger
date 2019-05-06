using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Protocols;
using Pinger.Logger;

namespace PingerTest.ProtocolTest
{
    [TestClass]
    public class IcmpProtocolTest
    {
        [TestMethod]
        public void FactoryTest()
        {
            //Arrange
            var protocolfactory = new Mock<IProtocolFactory>();
            var _icmpProtocol = protocolfactory.Object.CreateIcmpProtocol("localhost");
            var _tcpProtocol = protocolfactory.Object.CreaTcpProtocol("192.168.2.79:8080");
            var _httpProtocol = protocolfactory.Object.CreateHttpProtocol("http://yandex.ru", HttpStatusCode.Accepted);
            var ilog = new Mock<ILogger<Exception>>();

            //Act
            IcmpProtocol icmpProtocol = _icmpProtocol;
            TcpProtocol tcpProtocol = _tcpProtocol;
            HttpProtocol httpProtocol = _httpProtocol;

            //Assert
            Assert.IsNull(icmpProtocol);
            Assert.IsNull(tcpProtocol);
            Assert.IsNull(httpProtocol);
        }

    }
}
