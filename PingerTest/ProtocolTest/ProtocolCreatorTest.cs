using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.ConfigurationModule;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace PingerTest.ProtocolTest
{
    [TestClass]
    public class ProtocolCreatorTest
    {
        [TestMethod]
        public void CreationTest()
        {
            //Arrange
            var attr = CustomConfigAttribute.CreateConfigAttribute(new [] {"localhost", "1445", "icmp"});
            var mock = new Mock<IPinger>();
            var iProtocol = new Mock<IProtocol>();
            mock.Setup(x => x.Protocol).Returns(iProtocol.Object);
            iProtocol.Setup(x => x.Host).Returns("localhost");
            iProtocol.Setup(x => x.ProtocolName).Returns("Icmp");
            mock.Setup(x => x.Interval).Returns(1445);
            //var pinger = ProtocolCreator.CreateProtocol(attr);

            //Act
            IProtocol protocol = iProtocol.Object;
            IPinger _ipinger = mock.Object;
            IProtocol exprotocol = new IcmpProtocol("localhost");
            
            //Assert
            Assert.AreEqual(_ipinger.Protocol.Host, exprotocol.Host);
            Assert.AreEqual(exprotocol.Host, protocol.Host);
            Assert.AreEqual(_ipinger.Protocol.ProtocolName, exprotocol.ProtocolName);
            Assert.AreEqual(exprotocol.ProtocolName, protocol.ProtocolName);
            Assert.AreNotEqual(exprotocol, protocol);
        }
    }
}
