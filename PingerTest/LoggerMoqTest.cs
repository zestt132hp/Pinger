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
            var excLog = new Mock<ILogger<Exception>>();

            //act
            excLog.Setup(x => x.Write(It.IsAny<Exception>())).Verifiable();

            //assert
            ILogger<Exception> logger = excLog.Object;
            logger.Write(new Exception("Hello from Exception"));
            Mock.VerifyAll(excLog);
        }
    }
}
