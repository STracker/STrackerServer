// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using System.Web;

    using NUnit.Framework;
    using STrackerServer.Controllers;
    using STrackerServer.Models.Media;

    /// <summary>
    /// The television show tests.
    /// </summary>
    [TestFixture]
    public class TvShowTests
    {
        /// <summary>
        /// The controller.
        /// </summary>
        private readonly TvShowController controller = new TvShowController();

        /// <summary>
        /// The get.
        /// </summary>
        [Test]
        public void Get()
        {
            var result = controller.JsonGet(1);
            var tvshow = (TvShow)result.Data;

            /*
            Assert.AreEqual(tvshow.Id, 1);
            Assert.AreEqual(tvshow.Title, "The Walking Dead");
            Assert.AreEqual(tvshow.Description, "Police officer Rick Grimes leads a group of survivors in a world overrun by zombies.");
            Assert.AreEqual(tvshow.Creator, "Frank Darabont");
            */
        }

        [Test]
        public void GetSeason()
        {
            var result = controller.JsonGetSeason(1, 1);
            var season = (Season)result.Data;
            //Assert.AreEqual(season.Number, 1);
        }
    }
}
