using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using STrackerServer.Controllers;

namespace STrackerServer.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {

        private readonly HomeController _homeController = new HomeController();

        [Test]
        public void TestIndex()
        {
            Assert.AreEqual(_homeController.Index(), "Hello World!!");
        }
    }
}
