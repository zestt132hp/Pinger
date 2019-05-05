using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.PingerModule;

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

            //Act
            mock.Setup(x => x.Protocol);
            mock.Setup(x => x.Interval);

            //Assert
            IPinger pinger = mock.Object;
            Assert.AreEqual(0, pinger.Interval);
            Assert.IsTrue(pinger.Interval>-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PingerProcessorTest()
        {
            //Arrange
            var mock = new Mock<IPingerProcessor>();
            mock.Setup(x => x.Ping()).Verifiable();
            mock.Setup(x => x.Ping(It.Is<Int32>(z => z < 0))).Throws(new ArgumentException());
            mock.Setup(x => x.StopPing()).Verifiable();

            //Act
            IPingerProcessor processor = mock.Object;

            //Assert
            processor.Ping();
            processor.Ping(-1);
            processor.Ping(1);
            processor.StopPing();
            Mock.VerifyAll(mock);
        }
    }
}
