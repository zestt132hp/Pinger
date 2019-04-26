using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Pinger.PingerModule;
using Pinger.Protocols;
using Pinger.UI;

namespace PingerTest
{
    [TestClass]
    public class PingerMockTest
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ActionsPingerWorkTest()
        {
            //Arrange
            var mock = new Mock<IPinger>();
            var ui = new ConsoleWorkProcessUi();
            var ilog = ui.InjectKernel.Get<Pinger.Logger.ILogger>();

            //Act
            mock.Setup(x => x.StartWork(ilog)).Verifiable();
            mock.Setup(x=>x.StopWork()).Verifiable();
            mock.Setup(x => x.StartWork(null)).Throws<NullReferenceException>();

            //Assert
            IPinger ping = mock.Object;
            ping.StartWork(null);
            mock.VerifySet(x => x.StartWork(ilog));
            mock.VerifySet(x=>x.StopWork());
        }

        [TestMethod]
        public void PropertyPingerTest()
        {
            //Arrange 
            var mock = new Mock<IPinger>();
            IProtocol protocol = new IcmpProtocol("ya.ru") {Message = "Data" };
            IProtocol _protocol;
            _protocol = new HttpProtocol("http://ya.ru"){Message = "Data"};

            //Act
            mock.Setup(x => x.Protocol);
            mock.Setup(x => x.Interval);
            mock.SetupProperty(x => x.Interval, 0);
            mock.Setup(x => x.Protocol).Returns(_protocol);

            //Assert
            IPinger pinger = mock.Object;
            Assert.AreEqual(_protocol, pinger.Protocol);
            Assert.AreEqual(0, pinger.Interval);
            Assert.AreNotSame(pinger.Protocol, protocol);
            Assert.IsFalse(pinger.Protocol==protocol);
            Assert.IsTrue(pinger.Interval>-1);
        }
    }
}
