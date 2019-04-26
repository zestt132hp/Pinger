using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Logger;

namespace PingerTest
{
    [TestClass]
    public class LoggerMoqTest
    {
        [TestMethod]
        public void WriteMethodTest()
        {
            //arrange
            var mock = new Mock<ILogger>();

            //act
            mock.Setup(x => x.Write(It.IsAny<String>())).Verifiable();
            mock.Setup(x => x.Write(It.IsAny<Exception>())).Verifiable();

            //assert
            ILogger logger = mock.Object;
            logger.Write("Hello World");
            logger.Write(new Exception("Hello from Exception"));
            Mock.VerifyAll(mock);
        }
    }
}
