// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit tests for television shows operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Operations.Tests
{
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television shows operations.
    /// </summary>
    [TestFixture]
    public class TvShowsOperations
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ITvShowsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        public TvShowsOperations()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForUnitTests()))
            {
                this.repository = kernel.Get<ITvShowsRepository>();
            }
        }

        /// <summary>
        /// The read by IMDB id test.
        /// </summary>
        [Test]
        public void Read()
        {
            var tvshow = this.repository.Read("tt0098904");

            Assert.AreEqual(tvshow.TvShowId, "tt0098904");
            Assert.AreEqual(tvshow.Name, "Seinfeld");
            Assert.AreEqual(tvshow.Description, "Jerry Seinfeld is a very successful stand-up comedian, mainly because the people around him offer an endless supply of great material. His best friend is George Costanza, a bald, whiny loser who craves the kind of success Jerry has but is never willing to do what it takes to get it. Jerry's neighbor Kramer often barges into his apartment and imposes onto his life. In the second episode Jerry's former girlfriend Elaine Benes comes back into his life, and the four of them are able to form a friendship together. The episodes were rarely very plot-heavy, focusing more on mundane conversations and situations that could be found during everyday life in New York.");
            Assert.AreEqual(tvshow.FirstAired, "1989-07-05");
            Assert.AreEqual(tvshow.AirDay, "Thursday");
            Assert.AreEqual(tvshow.Runtime, 30);
        }

        /// <summary>
        /// The read by name test.
        /// </summary>
        [Test]
        public void ReadByName()
        {
            var tvshow = this.repository.ReadByName("Seinfeld").ElementAt(0);

            Assert.AreEqual(tvshow.Id, "tt0098904");
            Assert.AreEqual(tvshow.Name, "Seinfeld");
            Assert.AreEqual(tvshow.Uri, "tvshows/tt0098904");
        }

        /*
        /// <summary>
        /// The create.
        /// </summary>
        /// This test creates a fake television show, then update and delete it.
        [Test]
        public void CreateDelete()
        {
            // Create
            var tvshow = new TvShow("tt12345")
                {
                    Name = "FakeTvShow",
                    Description = "This is a fake television show information for testing...",
                    Runtime = 40
                };

            this.repository.Create(tvshow);

            var tvshowRead = this.repository.Read("tt12345");

            Assert.AreEqual(tvshowRead.Name, tvshow.Name);
            Assert.AreEqual(tvshowRead.Description, tvshow.Description);
            Assert.AreEqual(tvshowRead.Runtime, tvshow.Runtime);
            
            // Delete
            this.repository.Delete("tt12345");
            tvshowRead = this.repository.Read("tt12345");
            Assert.Null(tvshowRead);
        }
         */
    }
}