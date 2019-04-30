using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;
using Pinger.Protocols;

namespace PingerTest.ProtocolTest
{
    [TestClass]
    public class ProtocolTest
    {
       
        [TestMethod]
        public void SimulateIProtocolWorkingTest()
        {
            //Arrange
            var mocck = new Mock<IProtocol>();
            var ilog = new Mock<ILogger>();
            mocck.SetupGet(x => x.ProtocolName);
            mocck.Setup(x => x.SendRequest(ilog.Object)).Returns(new RequestStatus(false));

            //Act
            IProtocol protocol = mocck.Object;

            //Assert
           // protocol.Host = "yandex.ru";
           // protocol.Message = "data";
            Assert.AreNotSame("yandex.ru", protocol.Host);
            Assert.AreNotSame("data",protocol.Message);
            Assert.IsNull(protocol.ProtocolName);
            Assert.IsFalse(protocol.SendRequest(ilog.Object).IsSucces);
            mocck.Verify(protocolaction => protocolaction.SendRequest(ilog.Object));
            mocck.Verify(x=>x.Host);
            mocck.Verify(x=>x.Message);
            mocck.Verify(x=>x.ProtocolName);
        }
    }
}
