using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.ConfigurationModule;
using Pinger.PingerModule;
using Pinger.UI;
using Pinger = Pinger.PingerModule.Pinger;

namespace PingerTest
{
    [TestClass]
    public class MoqTests
    {
        private Dictionary<int, IPinger> listHost;

        [TestMethod]
        public void MoqConfigWorkerTest()
        {
            //arrange
            var _workconf = new Mock<IConfigWorker>();
            listHost = new Dictionary<int, IPinger>(10);
            listHost.Add(1, new global::Pinger.PingerModule.Pinger());
            listHost.Add(2, new global::Pinger.PingerModule.Pinger());
            listHost.Add(3, new global::Pinger.PingerModule.Pinger());

            //act
            _workconf.Setup(x => x.RemoveFromConfig(1));
            _workconf.Setup(x=>x.CreateConfig()).Verifiable();

            _workconf.Setup(x=>x.SaveInConfig()).Returns(false);
            _workconf.Setup(x => x.SaveInConfig("")).Returns(false);
            string[] array = {"www.google.com", "5", "http", "200"};
            _workconf.Setup(x => x.SaveInConfig(It.Is<string[]>(z=>array.Length>4 && array.Length<3))).Returns(false);
            _workconf.Setup(x => x.SaveInConfig(array)).Returns(true);

            _workconf.Setup(
                x => x.RemoveFromConfig(It.Is<Int32>(v => listHost.Count - 1 >= v && v>=0)))
                .Returns(true);
           /* _workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(v => v > 0))).Returns(true);

            _workconf.Setup(x => x.RemoveFromConfig(It.Is<Int32>(v => v < 0))).Returns(false);*/

            //assert
            IConfigWorker worker = _workconf.Object;
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
}
