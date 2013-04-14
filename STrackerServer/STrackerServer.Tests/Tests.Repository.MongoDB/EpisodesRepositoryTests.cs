// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit Tests for mongo episodes repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Tests.Repository.MongoDB
{
    using System;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Tests.NinjectModules;

    /// <summary>
    /// Episodes repository tests.
    /// </summary>
    [TestFixture]
    public class EpisodesRepositoryTests
    {
        /// <summary>
        /// Episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// Seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepositoryTests"/> class.
        /// </summary>
        public EpisodesRepositoryTests()
        {
            using (IKernel kernel = new StandardKernel(new TestModuleForMongoRepositories()))
            {
                this.episodesRepository = kernel.Get<IEpisodesRepository>();
                this.seasonsRepository = kernel.Get<ISeasonsRepository>();
            }
        }

        /// <summary>
        /// Create and Delete operations test.
        /// </summary>
        [Test(Description = "Create and Delete operations test for episodes mongo repository.")]
        public void CreateAndDelete()
        {
            // "tt0436992-test" is a id for one television show created specifically for testing and the season number 1 also was created for testing.
            var id = new Tuple<string, int, int>("tt0436992-test", 1, 1);

            /*
            * Delete
            * Also test if the episode synopse was removed from the season episodes synopses list.
            */
            Assert.IsTrue(this.episodesRepository.Delete(id));

            var episodeSynopse = this.seasonsRepository.Read(new Tuple<string, int>(id.Item1, id.Item2)).EpisodeSynopses.Find(e => e.Number == id.Item3);

            Assert.Null(episodeSynopse);

            /*
             * Create
             */
            var episode = new Episode(id) { Name = "Rose", Rating = 4 };

            Assert.IsTrue(this.episodesRepository.Create(episode));

            episodeSynopse = this.seasonsRepository.Read(new Tuple<string, int>(id.Item1, id.Item2)).EpisodeSynopses.Find(e => e.Number == id.Item3);

            Assert.NotNull(episodeSynopse);
        }

        /// <summary>
        /// Read operation test.
        /// </summary>
        [Test(Description = "Read operation test for episodes mongo repository.")]
        public void Read()
        {
            var id = new Tuple<string, int, int>("tt0436992-test", 1, 1);

            var episode = this.episodesRepository.Read(id);

            Assert.NotNull(episode);

            Assert.AreEqual(episode.Id, id.ToString());
            Assert.AreEqual(episode.Name, "Rose");
            Assert.AreEqual(episode.Rating, 4);
        }

        /// <summary>
        /// Update operation test.
        /// </summary>
        [Test(Description = "Update operation test for episodes mongo repository.")]
        public void Update()
        {
            var id = new Tuple<string, int, int>("tt0436992-test", 1, 1);

            var episode = this.episodesRepository.Read(id);

            Assert.NotNull(episode);

            // Update Name to Test.
            episode.Name = "Test";

            Assert.IsTrue(this.episodesRepository.Update(episode));

            episode = this.episodesRepository.Read(id);

            Assert.AreNotEqual(episode.Name, "Rose");
            Assert.AreEqual(episode.Name, "Test");

            var episodeSynopse = this.seasonsRepository.Read(new Tuple<string, int>(id.Item1, id.Item2)).EpisodeSynopses.Find(e => e.Number == id.Item3);

            Assert.NotNull(episodeSynopse);
            Assert.AreNotEqual(episodeSynopse.Name, "Rose");
            Assert.AreEqual(episodeSynopse.Name, "Test");

            // Update Name for old name, "Rose"
            episode.Name = "Rose";

            Assert.IsTrue(this.episodesRepository.Update(episode));

            episode = this.episodesRepository.Read(id);

            Assert.AreEqual(episode.Name, "Rose");

            episodeSynopse = this.seasonsRepository.Read(new Tuple<string, int>(id.Item1, id.Item2)).EpisodeSynopses.Find(e => e.Number == id.Item3);

            Assert.AreEqual(episodeSynopse.Name, "Rose");
        }
    }
}
