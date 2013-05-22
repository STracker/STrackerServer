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

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The seasons operations.
    /// </summary>
   // [TestFixture]
    public class SeasonsOperations
    {
        /*
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ISeasonsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsOperations"/> class.
        /// </summary>
        public SeasonsOperations()
        {
            using (IKernel kernel = new StandardKernel(new ModuleForSTracker()))
            {
                this.operations = kernel.Get<ISeasonsOperations>();
                this.tvshowsOperations = kernel.Get<ITvShowsOperations>();
            }
        }

        /// <summary>
        /// The read.
        /// </summary>
        [Test]
        public void Read()
        {
            var season = this.operations.Read(new Tuple<string, int>("tt0098904", 1));

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
            var seasons = this.operations.GetAllFromOneTvShow("tt0098904");

            Assert.AreEqual(((List<Season.SeasonSynopsis>)seasons).Count, 10);
        }

        /// <summary>
        /// The create update delete.
        /// </summary>
        [Test]
        public void CreateDelete()
        {
            // Create
            var tvshow = new TvShow("tt12346")
            {
                Name = "FakeTvShow",
                Description = "This is a fake television show information for testing...",
                Runtime = 40,
                Genres = new List<string> { "Comedy" }
            };
            this.tvshowsOperations.Create(tvshow);

            var season = new Season(new Tuple<string, int>("tt12346", 1));
            this.operations.Create(season);
            var seasonRead = this.operations.Read(new Tuple<string, int>("tt12346", 1));

            Assert.AreEqual(seasonRead.TvShowId, "tt12346");

            // Delete
            this.operations.Delete(new Tuple<string, int>("tt12346", 1));
            seasonRead = this.operations.Read(new Tuple<string, int>("tt12346", 1));

            Assert.Null(seasonRead);

            tvshow = this.tvshowsOperations.Read("tt12346");

            Assert.AreEqual(tvshow.SeasonSynopses.Count, 0);

            this.tvshowsOperations.Delete("tt12346");
            tvshow = this.tvshowsOperations.Read("tt12346");

            Assert.Null(tvshow);
        }
         * */
    }
}
