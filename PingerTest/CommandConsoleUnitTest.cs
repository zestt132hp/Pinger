using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.UI;

namespace PingerTest
{
    [TestClass]
    public class CommandConsoleUnitTest
    {
        [TestMethod]
        public void UiTestMethod()
        {
            var mock = new Mock<IUi>();
            mock.Setup(a=>a.SetUiSettings()).Verifiable();
            mock.Setup(a=>a.RunGui()).Verifiable();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
    "Введено пустое значение команды")]
        public void ConsoleUiTest()
        {
            ConsoleWorkProcessUi ui = new ConsoleWorkProcessUi();
            ui.OptionsToWork(null);
        }

        [TestMethod]
        public void CommandPingMethodTest()
        {
            ConsoleWorkProcessUi ui = new ConsoleWorkProcessUi();
            Assert.AreEqual(ui.CommandPing(""), false);
            Assert.AreEqual(ui.CommandPing(KeyOptions.Ping + "asfgsdf"), false);
            Assert.AreEqual(ui.CommandPing("pinger ping"), false);
            Assert.AreEqual(ui.CommandPing(KeyOptions.Remove), false);
            Assert.AreEqual(ui.CommandPing(null), false);
            Assert.AreNotEqual(ui.CommandPing(KeyOptions.Ping + " [3]"), true);
            Assert.AreEqual(ui.CommandPing("[3]"), false);
        }
        [TestMethod]
        public void CommandAddMethodTest()
        {
            
        }
    }
}
