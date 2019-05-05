using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.Protocols;

namespace PingerTest.ProtocolTest
{
    [TestClass]
    public class TcpProtocolTest
    {
        [TestMethod]
        public void TcpTest()
        {
            //Arrange
            var mock = new Mock<TcpProtocol>("192.168.7.29:980");
            var ilog = new Mock<ILogger<Exception>>();
            String expectedHost = "192.168.7.29";
            Int32 expectedPort = 980;
            var mock2 = new Mock<TcpProtocol>("10.200.224.201:5050"); //localhost

            //Act
            TcpProtocol protocol = mock.Object;
            TcpProtocol protocol2 = mock2.Object;

            //Assert
            Assert.AreEqual(expectedHost, protocol.Host);
            Assert.AreEqual(expectedPort, protocol.Port);
            Assert.IsNotNull(protocol.Message);
            Assert.IsNotNull(protocol.ProtocolName);
            Assert.IsFalse(protocol.SendRequest<Exception>(ilog.Object).IsSucces);
            Assert.IsFalse(protocol2.SendRequest<Exception>(ilog.Object).IsSucces); //порт закрыт :(
        }
    }
}
