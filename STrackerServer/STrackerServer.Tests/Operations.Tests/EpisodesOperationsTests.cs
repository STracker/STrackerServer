// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperationsTests.cs" company="STracker">
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

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes operations tests.
    /// </summary>
    [TestFixture]
    public class EpisodesOperationsTests
    {
        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepo;

        /// <summary>
        /// The seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepo;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IEpisodesRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperationsTests"/> class.
        /// </summary>
        public EpisodesOperationsTests()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForUnitTests()))
            {
                this.repository = kernel.Get<IEpisodesRepository>();
                this.seasonsRepo = kernel.Get<ISeasonsRepository>();
                this.tvshowsRepo = kernel.Get<ITvShowsRepository>();
            }
        }

        /// <summary>
        /// The read.
        /// </summary>
        [Test]
        public void Read()
        {
            var episode = this.repository.Read(new Tuple<string, int, int>("tt0098904", 1, 1));
            Assert.AreEqual(episode.Name, "The Seinfeld Chronicles");
            Assert.AreEqual(
                episode.Description,
                "Jerry tells George about a woman named Laura he met in Michigan who is coming to New York for a seminar. Jerry wonders if she has romantic intentions. The two continue to talk about her after they leave the luncheonette. Jerry then receives a telephone call from Laura, who asks if she can stay overnight at his apartment. Jerry invites her, but is still unsure whether or not her visit is intended to be romantic. Jerry and Laura arrive at the apartment. Laura then receives a call and when Laura gets off the phone she tells Jerry: \"Never get engaged.\" Jerry then realizes that he has no chance with Laura, but has already committed himself to an entire weekend with her. ");
        }

        /// <summary>
        /// The read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            var episodes = this.repository.GetAllFromOneSeason("tt0098904", 1);
            Assert.AreEqual(((List<Episode.EpisodeSynopsis>)episodes).Count, 5);
        }

        /// <summary>
        /// The create delete.
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
            this.seasonsRepo.Create(season);

            var episode = new Episode(new Tuple<string, int, int>("tt12345", 1, 1)) { Name = "FakeEpisode", Description = "This is a fake episode" };
            this.repository.Create(episode);

            var episodeRead = this.repository.Read(new Tuple<string, int, int>("tt12345", 1, 1));

            Assert.AreEqual(episodeRead.Name, "FakeEpisode");

            // Delete
            this.repository.Delete(new Tuple<string, int, int>("tt12345", 1, 1));
            episodeRead = this.repository.Read(new Tuple<string, int, int>("tt12345", 1, 1));

            Assert.Null(episodeRead);

            season = this.seasonsRepo.Read(new Tuple<string, int>("tt12345", 1));

            Assert.AreEqual(season.EpisodeSynopses.Count, 0);

            this.seasonsRepo.Delete(new Tuple<string, int>("tt12345", 1));
            tvshow = this.tvshowsRepo.Read("tt12345");

            Assert.AreEqual(tvshow.SeasonSynopses.Count, 0);

            this.tvshowsRepo.Delete("tt12345");
            tvshow = this.tvshowsRepo.Read("tt12345");

            Assert.Null(tvshow);
        }
    }
}
