// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episodes repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes repository tests.
    /// </summary>
    [TestFixture]
    public class EpisodesRepositoryTests
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
        /// The episodes repository.
        /// </summary>
        private readonly IEpisodesRepository episodesRepository;

        /// <summary>
        /// The television show new episodes repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository tvshowNewEpisodesRepository;

        /// <summary>
        /// The episode comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository episodeCommentsRepository;

        /// <summary>
        /// The episode ratings repository.
        /// </summary>
        private readonly IEpisodeRatingsRepository episodeRatingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepositoryTests"/> class.
        /// </summary>
        public EpisodesRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            this.seasonsRepository = kernel.Get<ISeasonsRepository>();
            this.episodesRepository = kernel.Get<IEpisodesRepository>();
            this.tvshowNewEpisodesRepository = kernel.Get<ITvShowNewEpisodesRepository>();
            this.episodeCommentsRepository = kernel.Get<IEpisodeCommentsRepository>();
            this.episodeRatingsRepository = kernel.Get<IEpisodeRatingsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var tvshow = Utils.CreateTvShow(Utils.CreateId());
            var season = Utils.CreateSeason(tvshow.Id, 1);
            var episode = Utils.CreateEpisode(tvshow.Id, 1, 1);


            Assert.True(this.tvshowsRepository.Create(tvshow));
            Assert.True(this.seasonsRepository.Create(season));
            Assert.True(this.episodesRepository.Create(episode));

            Assert.NotNull(this.episodeCommentsRepository.Read(episode.Id));
            Assert.NotNull(this.episodeRatingsRepository.Read(episode.Id));
        }

        /// <summary>
        /// Test create and add to television show new episodes.
        /// </summary>
        [Test]
        public void CreateUpCommingEpisode()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));

            var episode = Utils.CreateEpisode(id, 1, 1);
            episode.Date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            Assert.True(this.episodesRepository.Create(Utils.CreateEpisode(id, 1, 1)));

            Assert.True(this.tvshowNewEpisodesRepository.Read(id).Episodes.Any(synopsis => synopsis.Id.SeasonNumber == 1 && synopsis.Id.EpisodeNumber == 1));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));
            Assert.True(this.episodesRepository.Create(Utils.CreateEpisode(id, 1, 1)));
            Assert.False(this.episodesRepository.Create(Utils.CreateEpisode(id, 1, 1)));
        }

        /// <summary>
        /// Test create all.
        /// </summary>
        [Test]
        public void CreateAll()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));

            var episodes = new List<Episode> { Utils.CreateEpisode(id, 1, 1), Utils.CreateEpisode(id, 1, 2) };

            this.episodesRepository.CreateAll(episodes);

            foreach (var episode in episodes)
            {
                Assert.NotNull(this.episodesRepository.Read(episode.Id));
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
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));
            Assert.True(this.episodesRepository.Create(Utils.CreateEpisode(id, 1, 1)));
            Assert.NotNull(this.episodesRepository.Read(new Episode.EpisodeId { TvShowId = id, SeasonNumber = 1, EpisodeNumber = 1 }));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));
            Assert.Null(this.episodesRepository.Read(new Episode.EpisodeId { TvShowId = id, SeasonNumber = 1, EpisodeNumber = 1 }));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.episodesRepository.ReadAll());
        }

        /// <summary>
        /// The update.
        /// </summary>
        [Test]
        public void Update()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));

            var episode = Utils.CreateEpisode(id, 1, 1);

            Assert.True(this.episodesRepository.Create(episode));

            episode.Poster = "fake_poster";
            episode.Name = "fake_name";

            Assert.True(this.episodesRepository.Update(episode));

            episode = this.episodesRepository.Read(episode.Id);

            Assert.AreEqual("fake_poster", episode.Poster);
            Assert.AreEqual("fake_name", episode.Name);

            var season = this.seasonsRepository.Read(new Season.SeasonId { TvShowId = id, SeasonNumber = 1 });

            Assert.True(season.Episodes.Any(synopsis => synopsis.Id.EpisodeNumber == 1 && episode.Name.Equals("fake_name") && episode.Poster.Equals("fake_poster")));
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason(id, 1)));
            Assert.True(this.episodesRepository.Create(Utils.CreateEpisode(id, 1, 1)));

            Assert.True(this.episodesRepository.Delete(new Episode.EpisodeId { TvShowId = id, SeasonNumber = 1, EpisodeNumber = 1 }));

            Assert.Null(this.episodesRepository.Read(new Episode.EpisodeId { TvShowId = id, SeasonNumber = 1, EpisodeNumber = 1 }));
        }
    }
}
