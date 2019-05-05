using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Protocols;
using Pinger.Logger;
using Moq;

namespace PingerTest.ProtocolTest
{
    [TestClass]
    public class HttpProtocolTest
    {
        [TestMethod]
        public void HttpTest()
        {
            //Arrange
            var mock = new Mock<HttpProtocol>("yandex.ru", HttpStatusCode.BadRequest);
            var ilog = new Mock<ILogger<Exception>>();
            String expectedProtocolname = "http/https";
            String expectedhost = "http://yandex.ru";
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            //Act
            HttpProtocol protocol = mock.Object;

            //Assert
            Assert.AreEqual(expectedProtocolname, protocol.ProtocolName.ToLowerInvariant());
            Assert.AreEqual(expectedhost, protocol.Host);
            Assert.AreEqual((Int16)statusCode, protocol.StatusCode);
            Assert.IsFalse(protocol.SendRequest<Exception>(ilog.Object).IsSucces);
        }
    }
}
