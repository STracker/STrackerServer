﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The seasons repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The seasons repository tests.
    /// </summary>
    [TestFixture]
    public class SeasonsRepositoryTests
    {
        /// <summary>
        /// The seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsRepositoryTests"/> class.
        /// </summary>
        public SeasonsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            this.seasonsRepository = kernel.Get<ISeasonsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 3)));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 3)));
            Assert.False(this.seasonsRepository.Create(Utils.CreateSeason(id, 3)));
        }

        /// <summary>
        /// Test create all.
        /// </summary>
        [Test]
        public void CreateAll()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));

            var seasonList = new List<Season> { Utils.CreateSeason(id, 1), Utils.CreateSeason(id, 2) };

            this.seasonsRepository.CreateAll(seasonList);

            foreach (var season in seasonList)
            {
                Assert.NotNull(this.seasonsRepository.Read(season.Id));
            }
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 3)));
            Assert.NotNull(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = id, SeasonNumber = 3 }));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.Null(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = id, SeasonNumber = 3 }));
            Assert.Null(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = "fake_id", SeasonNumber = 3 }));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.seasonsRepository.ReadAll());
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            Assert.Throws<NotSupportedException>(() => this.seasonsRepository.Update(new Season()));
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 3)));
            Assert.True(this.seasonsRepository.Delete(new Season.SeasonId { TvShowId = id, SeasonNumber = 3 }));
            Assert.Null(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = id, SeasonNumber = 3 }));
        }

        /// <summary>
        /// Test add episode.
        /// </summary>
        [Test]
        public void AddEpisode()
        {
            var id = Utils.CreateId();
            var seasonId = new Season.SeasonId { TvShowId = id, SeasonNumber = 1 };
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));

            Assert.True(this.seasonsRepository.AddEpisode(seasonId, Utils.CreateEpisode(id, 1, 2).GetSynopsis()));

            var season = this.seasonsRepository.Read(seasonId);

            Assert.NotNull(season);
            Assert.True(season.Episodes.Any(synopsis => synopsis.Id.EpisodeNumber == 2));
        }

        /// <summary>
        /// Test remove episode.
        /// </summary>
        [Test]
        public void RemoveEpisode()
        {
            var id = Utils.CreateId();
            var seasonId = new Season.SeasonId { TvShowId = id, SeasonNumber = 1 };

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));

            Assert.True(this.seasonsRepository.RemoveEpisode(seasonId, Utils.CreateEpisode(id, 1, 1).GetSynopsis()));

            var season = this.seasonsRepository.Read(seasonId);

            Assert.NotNull(season);
            Assert.False(season.Episodes.Any(synopsis => synopsis.Id.EpisodeNumber == 1));
        }
    }
}
