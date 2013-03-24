using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using STrackerServer.Controllers;
using STrackerServer.Models;

namespace STrackerServer.Tests
{
    [TestFixture]
    public class TvShowTests
    {

        private readonly TvShowController _controller = new TvShowController();

        [Test]
        public void Get()
        {
            var result = _controller.Show(1);

            var tvshow = (TvShow) result.Data;

            Assert.AreEqual(tvshow.ImdbId, 1);
            Assert.AreEqual(tvshow.Title, "The Walking Dead");
        }
    }
}
