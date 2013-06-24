// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit tests for television shows operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Operations.Tests
{
    using System;
    using System.Collections.Generic;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The seasons operations.
    /// </summary>
    [TestFixture]
    public class SeasonsOperations
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ISeasonsRepository repository;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsOperations"/> class.
        /// </summary>
        public SeasonsOperations()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForUnitTests()))
            {
                this.repository = kernel.Get<ISeasonsRepository>();
                this.tvshowsRepo = kernel.Get<ITvShowsRepository>();
            }
        }

        /// <summary>
        /// The read.
        /// </summary>
        [Test]
        public void Read()
        {
            var season = this.repository.Read(new Tuple<string, int>("tt0098904", 1));

            Assert.AreEqual(season.SeasonNumber, 1);
            Assert.AreEqual(season.TvShowId, "tt0098904");
            Assert.AreEqual(season.EpisodeSynopses.Count, 5);
        }

        /// <summary>
        /// The read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            var seasons = this.repository.GetAllFromOneTvShow("tt0098904");

            Assert.AreEqual(((List<Season.SeasonSynopsis>)seasons).Count, 10);
        }

        /// <summary>
        /// The create update delete.
        /// </summary>
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
            this.tvshowsRepo.Create(tvshow);

            var season = new Season(new Tuple<string, int>("tt12345", 1));
            this.repository.Create(season);
            var seasonRead = this.repository.Read(new Tuple<string, int>("tt12345", 1));

            Assert.AreEqual(seasonRead.TvShowId, "tt12345");

            // Delete
            this.repository.Delete(new Tuple<string, int>("tt12345", 1));
            seasonRead = this.repository.Read(new Tuple<string, int>("tt12345", 1));

            Assert.Null(seasonRead);

            tvshow = this.tvshowsRepo.Read("tt12345");

            Assert.AreEqual(tvshow.SeasonSynopses.Count, 0);

            this.tvshowsRepo.Delete("tt12345");
            tvshow = this.tvshowsRepo.Read("tt12345");
              
            Assert.Null(tvshow);
        }
    }
}
