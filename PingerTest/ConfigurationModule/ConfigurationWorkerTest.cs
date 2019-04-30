using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.ConfigurationModule;

namespace PingerTest
{
    [TestClass]
    public class ConfigurationWorkerTest
    {

        [TestMethod]
        public void WorkerMethodAddTest()
        {
            var moq = new Mock<IConfigWorker>();
            moq.Setup(x => x.SaveInConfig()).Returns(false);
        }
    }
}
