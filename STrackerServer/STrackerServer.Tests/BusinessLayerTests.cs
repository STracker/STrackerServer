// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusinessLayerTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit Tests for business layer of the project STracker.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using Ninject;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Facades;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The business layer  tests.
    /// </summary>
    [TestFixture]
    public class BusinessLayerTests
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The seasons operations.
        /// </summary>
        private ISeasonsOperations seasonsOperations;

        /// <summary>
        /// The episodes operations.
        /// </summary>
        private IEpisodesOperations episodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLayerTests"/> class.
        /// </summary>
        public BusinessLayerTests()
        {
            using (IKernel kernel = new StandardKernel(new TestModule()))
            {
                this.tvshowsOperations = kernel.Get<TvShowsOperations>();
            }
        }

        /// <summary>
        /// Test for television shows get operation.
        /// </summary>
        [Test]
        public void TvShowsGetOperation()
        {
            const string Id = "tt1520211";
            const string Name = "The Walking Dead";
            const string Descr = "Police officer Rick Grimes leads a group of survivors in a world overrun by zombies.";
            const int Rating = 4;

            var tvshowC = new TvShow(Id)
                {
                    Name = Name,
                    Description = Descr,
                    Rating = Rating
                };

            var create = this.tvshowsOperations.Create(tvshowC);

            /*
             * Get test.
             */

            var tvshow = this.tvshowsOperations.Read(Id);

            Assert.AreEqual(tvshow.Id, Id);
            Assert.AreEqual(tvshow.Name, Name);
            Assert.AreEqual(tvshow.Description, Descr);
            Assert.AreEqual(tvshow.Rating, Rating);

            // Realize that fails.
            Assert.AreNotEqual(tvshow.Id, "tt123456");
        }
    }
}
