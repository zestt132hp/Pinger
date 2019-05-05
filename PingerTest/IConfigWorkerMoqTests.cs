using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog.Config;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.PingerModule;
using Pinger.Protocols;

namespace PingerTest
{
    [TestClass]
    public class ConfigWorkerMoqTests
    {
        private Dictionary<Int32, IPinger> _listHost;
        private readonly Dictionary<Int32, IPinger> _list = null;

        [TestMethod]
        public void MoqConfigWorkerTest()
        {
            //arrange
            var workconf = new Mock<IConfigWorker>();
            var protocol = new Mock<IProtocol>();
            var excLog = new Mock<ILogger<Exception>>();
            var logger = new Mock<ILogger<String>>();
            _listHost = new Dictionary<Int32, IPinger>(10)
            {
                {1, new Pinger.PingerModule.Pinger(protocol.Object, excLog.Object, logger.Object)},
                {2, new Pinger.PingerModule.Pinger(protocol.Object, excLog.Object, logger.Object)},
                {3, new Pinger.PingerModule.Pinger(protocol.Object, excLog.Object, logger.Object)}
            };

            //act
            workconf.Setup(x => x.RemoveFromConfig(1));
            workconf.Setup(x=>x.CreateConfig()).Verifiable();

            workconf.Setup(x=>x.SaveInConfig()).Returns(false);
            workconf.Setup(x => x.SaveInConfig("")).Returns(false);
            String[] array = {"www.google.com", "5", "http", "200"};
            workconf.Setup(x => x.SaveInConfig(It.Is<String[]>(z=>array.Length>4 && array.Length<3))).Returns(false);
            workconf.Setup(x => x.SaveInConfig(array)).Returns(true);

            workconf.Setup(
                x => x.RemoveFromConfig(It.Is<Int32>(v => _listHost.Count == 0))).Returns(false);
            workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(v => _list==null))).Returns(false);
            workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(v => _listHost.Count > v)))
                .Returns(true);
            workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(i => i < 0))).Returns(false);

            //assert
            IConfigWorker worker = workconf.Object;
            worker.CreateConfig();
            Assert.AreEqual(worker.SaveInConfig(""), false);
            Assert.AreEqual(worker.SaveInConfig("www.google.com", "5", "http", "200"), true);
            Assert.AreEqual(worker.SaveInConfig("www.google.com", "5", "http", "200",""), false);
            Assert.AreEqual(worker.SaveInConfig("www.google.com", "5"), false);
            Assert.AreEqual(worker.SaveInConfig(), false);
            Assert.AreEqual(worker.RemoveFromConfig(0), true);
            Assert.AreEqual(worker.RemoveFromConfig(2), true);
            Assert.AreEqual(worker.RemoveFromConfig(-1), false);
            Assert.AreEqual(worker.RemoveFromConfig(11), false);
        }
    }

    [TestClass]
    public class NlogCpnfigurationTest
    {
        [TestMethod]
        public void NlogConfigTest()
        {
            //Arrange
            var mock = new Mock<IConfigurationNlog>();

            //Act
            mock.Setup(x => x.GetLogConfiguration("LogName")).Returns(new LoggingConfiguration());

            //Assert
            IConfigurationNlog conf = mock.Object;
            var nConf = conf.GetLogConfiguration("LogName");
            Assert.AreEqual(nConf, mock.Object.GetLogConfiguration("LogName"));
        }
    }
}
