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
    using System.Collections.Generic;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The television shows operations.
    /// </summary>
    [TestFixture]
    public class TvShowsOperations
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ITvShowsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsOperations"/> class.
        /// </summary>
        public TvShowsOperations()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForSTracker()))
            {
                this.operations = kernel.Get<ITvShowsOperations>();
            }
        }

        /// <summary>
        /// The read by IMDB id test.
        /// </summary>
        [Test]
        public void Read()
        {
            var tvshow = this.operations.Read("tt0098904");

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
            var tvshow = this.operations.ReadByName("Seinfeld");

            Assert.AreEqual(tvshow.TvShowId, "tt0098904");
            Assert.AreEqual(tvshow.Name, "Seinfeld");
            Assert.AreEqual(tvshow.Description, "Jerry Seinfeld is a very successful stand-up comedian, mainly because the people around him offer an endless supply of great material. His best friend is George Costanza, a bald, whiny loser who craves the kind of success Jerry has but is never willing to do what it takes to get it. Jerry's neighbor Kramer often barges into his apartment and imposes onto his life. In the second episode Jerry's former girlfriend Elaine Benes comes back into his life, and the four of them are able to form a friendship together. The episodes were rarely very plot-heavy, focusing more on mundane conversations and situations that could be found during everyday life in New York.");
            Assert.AreEqual(tvshow.FirstAired, "1989-07-05");
            Assert.AreEqual(tvshow.AirDay, "Thursday");
            Assert.AreEqual(tvshow.Runtime, 30);
        }

        /// <summary>
        /// The read by genre.
        /// </summary>
        [Test]
        public void ReadByGenre()
        {
            // Create fake television show
            var tvshow = new TvShow("tt12345")
            {
                Name = "FakeTvShow",
                Description = "This is a fake television show information for testing...",
                Runtime = 40,
                Genres = new List<string> { "FakeGenre" }
            };

            this.operations.Create(tvshow);

            // Assert
            var tvshowRead = this.operations.ReadAllByGenre("FakeGenre").First();

            Assert.AreEqual(tvshowRead.Name, tvshow.Name);
            Assert.AreEqual(tvshowRead.Id, tvshow.TvShowId);

            // Delete
            this.operations.Delete("tt12345");

            Assert.Null(this.operations.Read("tt12345"));
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// This test creates a fake television show, then update and delete it.
        [Test]
        public void CreateUpdateDelete()
        {
            // Create
            var tvshow = new TvShow("tt12345")
                {
                    Name = "FakeTvShow",
                    Description = "This is a fake television show information for testing...",
                    Runtime = 40,
                    Genres = new List<string> { "Comedy" }
                };

            this.operations.Create(tvshow);

            var tvshowRead = this.operations.Read("tt12345");

            Assert.AreEqual(tvshowRead.Name, tvshow.Name);
            Assert.AreEqual(tvshowRead.Description, tvshow.Description);
            Assert.AreEqual(tvshowRead.Runtime, tvshow.Runtime);

            // Update
            tvshow.Runtime = 60;
            this.operations.Update(tvshow);
            
            tvshowRead = this.operations.Read("tt12345");
            
            Assert.AreEqual(tvshowRead.Runtime, tvshow.Runtime);
            
            // Delete
            this.operations.Delete("tt12345");

            tvshowRead = this.operations.Read("tt12345");

            Assert.Null(tvshowRead);
        }
    }
}