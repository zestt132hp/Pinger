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
            mock.Setup(a=>a.RunGui()).Verifiable();
        }
    }
}
