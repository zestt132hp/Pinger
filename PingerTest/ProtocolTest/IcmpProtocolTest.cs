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
        public void IcmpTest()
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

    }
}
