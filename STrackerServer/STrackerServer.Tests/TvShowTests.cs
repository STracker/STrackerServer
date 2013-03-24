// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
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
            var result = this.controller.ShowJson(1);
            var tvshow = (TvShow)result.Data;

            Assert.AreEqual(tvshow.Id, 1);
            Assert.AreEqual(tvshow.Title, "The Walking Dead");
        }
    }
}
