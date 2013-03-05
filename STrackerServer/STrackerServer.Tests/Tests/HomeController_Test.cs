using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STrackerServer.Controllers;

namespace STrackerServer.Tests
{
    [TestClass]
    public class HomeController_Test
    {

        private HomeController _homeController = new HomeController();

        [TestMethod]
        public void TestIndex()
        {   
            Assert.AreEqual(_homeController.Index(), "Hello World");
        }
    }
}
