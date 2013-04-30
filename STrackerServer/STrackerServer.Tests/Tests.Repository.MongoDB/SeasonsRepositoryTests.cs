// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit Tests for mongo seasons repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Tests.Repository.MongoDB
{
    using System;
    using System.Collections.Generic;

    using global::MongoDB.Driver;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Tests.NinjectModules;

    /// <summary>
    /// Seasons repository tests.
    /// </summary>
    [TestFixture]
    public class SeasonsRepositoryTests
    {
        /// <summary>
        /// Seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// Television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsRepositoryTests"/> class.
        /// </summary>
        public SeasonsRepositoryTests()
        {
            using (IKernel kernel = new StandardKernel(new TestModuleForMongoRepositories()))
            {
                this.seasonsRepository = kernel.Get<ISeasonsRepository>();
                this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            }
        }

        /// <summary>
        /// Create and Delete operations test.
        /// </summary>
        /// The season number 1 alaways exists for testing, so in this case first test the Delete
        /// then the Create operation.
        [Test(Description = "Create and Delete operations test for seasons mongo repository.")]
        public void CreateAndDelete()
        {
            // "tt0436992-test" is a id for one television show created specifically for testing.
            var id = new Tuple<string, int>("tt0436992-test", 1);

            /*
             * Delete
             * Also test if the season synopse was removed from the television show seasons synopses list.
             */
            Assert.IsTrue(this.seasonsRepository.Delete(id));

            var seasonSynopse = this.tvshowsRepository.Read(id.Item1).SeasonSynopses.Find(s => s.Number == id.Item2);

            Assert.Null(seasonSynopse);

            /*
             * Create
             */
            var season = new Season(id) { EpisodeSynopses = new List<Episode.EpisodeSynopsis>() };
            season.EpisodeSynopses.Add(new Episode.EpisodeSynopsis { Number = 1, Name = "Rose" });

            Assert.IsTrue(this.seasonsRepository.Create(season));

            seasonSynopse = this.tvshowsRepository.Read(id.Item1).SeasonSynopses.Find(s => s.Number == id.Item2);

            Assert.NotNull(seasonSynopse);
        }

        /// <summary>
        /// Create fail operation test.
        /// </summary>
        [Test(Description = "Create operation test for seasons mongo repository. Fails because the try to create one season with one id that already exists."), ExpectedException(typeof(WriteConcernException))]
        public void CreateFail()
        {
            var season = new Season(new Tuple<string, int>("tt0436992-test", 1));

            this.seasonsRepository.Create(season);
        }

        /// <summary>
        /// Read operation test.
        /// </summary>
        [Test(Description = "Read operation test for seasons mongo repository.")]
        public void Read()
        {
            var id = new Tuple<string, int>("tt0436992-test", 1);

            var season = this.seasonsRepository.Read(id);

            Assert.NotNull(season);

            Assert.AreEqual(season.Id, id.ToString());
        }
    }
}
