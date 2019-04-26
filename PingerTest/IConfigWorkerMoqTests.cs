using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog.Config;
using Pinger.ConfigurationModule;
using Pinger.Logger;
using Pinger.PingerModule;

namespace PingerTest
{
    [TestClass]
    public class ConfigWorkerMoqTests
    {
        private Dictionary<int, IPinger> _listHost;
        private Dictionary<int, IPinger> list = null;

        [TestMethod]
        public void MoqConfigWorkerTest()
        {
            //arrange
            var workconf = new Mock<IConfigWorker>();
            _listHost = new Dictionary<int, IPinger>(10);
            _listHost.Add(1, new Pinger.PingerModule.Pinger());
            _listHost.Add(2, new Pinger.PingerModule.Pinger());
            _listHost.Add(3, new Pinger.PingerModule.Pinger());

            //act
            workconf.Setup(x => x.RemoveFromConfig(1));
            workconf.Setup(x=>x.CreateConfig()).Verifiable();

            workconf.Setup(x=>x.SaveInConfig()).Returns(false);
            workconf.Setup(x => x.SaveInConfig("")).Returns(false);
            string[] array = {"www.google.com", "5", "http", "200"};
            workconf.Setup(x => x.SaveInConfig(It.Is<string[]>(z=>array.Length>4 && array.Length<3))).Returns(false);
            workconf.Setup(x => x.SaveInConfig(array)).Returns(true);

            workconf.Setup(
                x => x.RemoveFromConfig(It.Is<Int32>(v => _listHost.Count == 0))).Returns(false);
            workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(v => list==null))).Returns(false);
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
