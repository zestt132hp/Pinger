using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.Protocols;

namespace PingerTest
{
    [TestClass]
    public class ProtocolTest
    {
        [TestMethod]
        public void HttpProtocolTest()
        {
            //Arrange
            var mock = new Mock<HttpProtocol>("yandex.ru", HttpStatusCode.BadRequest);
            var ilog = new Mock<ILogger>();
            string expectedProtocolname = "http/https";
            string expectedhost = "http://yandex.ru";
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            //Act
            HttpProtocol protocol = mock.Object;

            //Assert
            Assert.AreEqual(expectedProtocolname, protocol.ProtocolName.ToLowerInvariant());
            Assert.AreEqual(expectedhost, protocol.Host);
            Assert.AreEqual((Int16) statusCode, protocol.StatusCode);
            Assert.IsFalse(protocol.SendRequest(ilog.Object).IsSucces);
                //установить в true при наличии интернет соединения
        }

        [TestMethod]
        public void TcpProtocolTest()
        {
            //Arrange
            var mock = new Mock<TcpProtocol>("192.168.7.29:980");
            var ilog = new Mock<ILogger>();
            string expectedHost = "192.168.7.29";
            int expectedPort = 980;
            var mock2 = new Mock<TcpProtocol>("10.200.224.201:5050"); //localhost

            //Act
            TcpProtocol protocol = mock.Object;
            TcpProtocol protocol2 = mock2.Object;

            //Assert
            Assert.AreEqual(expectedHost, protocol.Host);
            Assert.AreEqual(expectedPort, protocol.Port);
            Assert.IsNotNull(protocol.Message);
            Assert.IsNotNull(protocol.ProtocolName);
            Assert.IsFalse(protocol.SendRequest(ilog.Object).IsSucces);
            Assert.IsFalse(protocol2.SendRequest(ilog.Object).IsSucces); //порт закрыт :(
        }

        [TestMethod]
        public void IcmpProtocolTest()
        {
            //Arrange
            var mock = new Mock<IcmpProtocol>("localhost");
            var ilog = new Mock<ILogger>();
            string expectedHost = "localhost";
            string expectedMessage = "DataTest";

            //Act
            IcmpProtocol protocol = mock.Object;

            //Assert
            Assert.AreEqual(expectedHost, protocol.Host);
            Assert.AreEqual(expectedMessage, protocol.Message);
            Assert.IsNotNull(protocol.Message);
            Assert.IsNotNull(protocol.ProtocolName);
            Assert.IsTrue(protocol.SendRequest(ilog.Object).IsSucces);
        }

        [TestMethod]
        public void SimulateIProtocolWorkingTest()
        {
            //Arrange
            var mocck = new Mock<IProtocol>();
            var ilog = new Mock<ILogger>();
            mocck.SetupProperty(x => x.Host);
            mocck.SetupProperty(x => x.Message);
            mocck.SetupGet(x => x.ProtocolName);
            mocck.Setup(x => x.SendRequest(ilog.Object)).Returns(new RequestStatus(false));

            //Act
            IProtocol protocol = mocck.Object;

            //Assert
            protocol.Host = "yandex.ru";
            protocol.Message = "data";
            Assert.AreSame("yandex.ru", protocol.Host);
            Assert.AreSame("data",protocol.Message);
            Assert.IsNull(protocol.ProtocolName);
            Assert.IsFalse(protocol.SendRequest(ilog.Object).IsSucces);
            mocck.Verify(protocolaction => protocolaction.SendRequest(ilog.Object));
            mocck.Verify(x=>x.Host);
            mocck.Verify(x=>x.Message);
            mocck.Verify(x=>x.ProtocolName);
        }
    }
}
