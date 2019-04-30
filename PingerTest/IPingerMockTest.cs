using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Pinger.Logger;
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
            ui.InjectKernel.Get<ILogger>();

            //Act
            mock.Setup(x => x.StartWork()).Verifiable();
            mock.Setup(x=>x.StopWork()).Verifiable();
            mock.Setup(x => x.StartWork()).Throws<NullReferenceException>();

            //Assert
            IPinger ping = mock.Object;
            ping.StartWork();
            mock.VerifySet(x => x.StartWork());
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
            mock.Setup(x => x.Protocol).Returns(_protocol);

            //Assert
            IPinger pinger = mock.Object;
            Assert.AreEqual(_protocol, pinger.Protocol);
            Assert.AreEqual(0, pinger.Interval);
            Assert.AreNotSame(pinger.Protocol, protocol);
            Assert.IsFalse(pinger.Protocol==protocol);
            Assert.IsTrue(pinger.Interval>-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PingerProcessorTest()
        {
            //Arrange
            var mock = new Mock<IPingerProcessor>();
            var logger = new Mock<ILogger>();
            mock.Setup(x => x.Ping(logger.Object)).Verifiable();
            mock.Setup(x => x.Ping(It.Is<Int32>(z => z < 0), logger.Object)).Throws(new ArgumentException());
            mock.Setup(x => x.StopPing()).Verifiable();

            //Act
            IPingerProcessor processor = mock.Object;

            //Assert
            processor.Ping(logger.Object);
            processor.Ping(-1, logger.Object);
            processor.Ping(1, logger.Object);
            processor.StopPing();
            Mock.VerifyAll(mock);
        }
    }
}
