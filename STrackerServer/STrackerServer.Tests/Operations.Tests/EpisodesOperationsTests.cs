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

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.NinjectDependencies;

    /// <summary>
    /// The episodes operations tests.
    /// </summary>
    [TestFixture]
    public class EpisodesOperationsTests
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsoperations;

        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IEpisodesOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperationsTests"/> class.
        /// </summary>
        public EpisodesOperationsTests()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForSTracker()))
            {
                this.operations = kernel.Get<IEpisodesOperations>();
                this.seasonsoperations = kernel.Get<ISeasonsOperations>();
                this.tvshowsOperations = kernel.Get<ITvShowsOperations>();
            }
        }

        /// <summary>
        /// The read.
        /// </summary>
        [Test]
        public void Read()
        {
            var episode = this.operations.Read(new Tuple<string, int, int>("tt0098904", 1, 1));
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
            var episodes = this.operations.GetAllFromOneSeason("tt0098904", 1);
            Assert.AreEqual(((List<Episode.EpisodeSynopsis>)episodes).Count, 5);
        }

        /// <summary>
        /// The create update delete.
        /// </summary>
        [Test]
        public void CreateUpdateDelete()
        {
            // Create
            var tvshow = new TvShow("tt12347")
            {
                Name = "FakeTvShow",
                Description = "This is a fake television show information for testing...",
                Runtime = 40,
                Genres = new List<string> { "Comedy" }
            };
            this.tvshowsOperations.Create(tvshow);

            var season = new Season(new Tuple<string, int>("tt12347", 1));
            this.seasonsoperations.Create(season);

            var episode = new Episode(new Tuple<string, int, int>("tt12347", 1, 1)) { Name = "FakeEpisode", Description = "This is a fake episode" };
            this.operations.Create(episode);

            var episodeRead = this.operations.Read(new Tuple<string, int, int>("tt12347", 1, 1));

            Assert.AreEqual(episodeRead.Name, "FakeEpisode");
            Assert.AreEqual(episode.Rating, 0);

            // Update
            episode.Rating = 5;
            this.operations.Update(episode);
            episodeRead = this.operations.Read(new Tuple<string, int, int>("tt12347", 1, 1));

            Assert.AreEqual(episodeRead.Rating, 5);

            // Delete
            this.operations.Delete(new Tuple<string, int, int>("tt12347", 1, 1));
            episodeRead = this.operations.Read(new Tuple<string, int, int>("tt12347", 1, 1));

            Assert.Null(episodeRead);

            season = this.seasonsoperations.Read(new Tuple<string, int>("tt12347", 1));

            Assert.AreEqual(season.EpisodeSynopses.Count, 0);

            this.seasonsoperations.Delete(new Tuple<string, int>("tt12347", 1));
            tvshow = this.tvshowsOperations.Read("tt12347");

            Assert.AreEqual(tvshow.SeasonSynopses.Count, 0);

            this.tvshowsOperations.Delete("tt12347");
            tvshow = this.tvshowsOperations.Read("tt12347");

            Assert.Null(tvshow);
        }
    }
}
