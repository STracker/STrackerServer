using STrackerServer.Controllers;
using NUnit.Framework;
namespace STrackerServer.Tests.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {

        private readonly HomeController _homeController = new HomeController();

        [Test]
        public void TestIndex()
        {   
            Assert.AreEqual(_homeController.Index(), "Hello World");
        }
    }
}
