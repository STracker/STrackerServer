using STrackerServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace STrackerServer.Tests.Tests
{
    [TestClass]
    public class HomeControllerTest
    {

        private readonly HomeController _homeController = new HomeController();

        [TestMethod]
        public void TestIndex()
        {   
            Assert.AreEqual(_homeController.Index(), "Hello World");
        }
    }
}
