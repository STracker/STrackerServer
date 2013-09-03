// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowsRepositoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories.Tests
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;

    /// <summary>
    /// The television shows repository tests.
    /// </summary>
    [TestFixture]
    public class TvShowsRepositoryTests
    {
        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The genres repository.
        /// </summary>
        private readonly IGenresRepository genresRepository;

        /// <summary>
        /// The television show comments repository.
        /// </summary>
        private readonly ITvShowCommentsRepository tvshowCommentsRepository;

        /// <summary>
        /// The television show ratings repository.
        /// </summary>
        private readonly ITvShowRatingsRepository tvshowRatingsRepository;

        /// <summary>
        /// The television show new episodes repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository tvshowNewEpisodesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRepositoryTests"/> class.
        /// </summary>
        public TvShowsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            this.tvshowCommentsRepository = kernel.Get<ITvShowCommentsRepository>();
            this.tvshowRatingsRepository = kernel.Get<ITvShowRatingsRepository>();
            this.genresRepository = kernel.Get<IGenresRepository>();
            this.tvshowNewEpisodesRepository = kernel.Get<ITvShowNewEpisodesRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("1")));
            Assert.True(this.genresRepository.Read("Genre1").Tvshows.Any(synopsis => synopsis.Id.Equals("1")));

            Assert.NotNull(this.tvshowRatingsRepository.Read("1"));
            Assert.NotNull(this.tvshowCommentsRepository.Read("1"));
            Assert.NotNull(this.tvshowNewEpisodesRepository.Read("1"));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("2")));
            Assert.False(this.tvshowsRepository.Create(Utils.CreateTvShow("2")));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("3")));
            Assert.NotNull(this.tvshowsRepository.Read("3"));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.tvshowsRepository.Read("123"));
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("8")));

            var tvshow = this.tvshowsRepository.Read("8");
            Assert.NotNull(tvshow);

            tvshow.Name = "Name2";

            Assert.True(this.tvshowsRepository.Update(tvshow));

            Assert.True(this.genresRepository.Read("Genre2").Tvshows.Any(synopsis => synopsis.Id.Equals("8") && synopsis.Name.Equals("Name2")));

            Assert.AreEqual(this.tvshowNewEpisodesRepository.Read("8").TvShow.Name, "Name2");
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("4")));
            Assert.True(this.tvshowsRepository.Delete("4"));
            Assert.Null(this.tvshowsRepository.Read("4"));
        }

        /// <summary>
        /// The read by name.
        /// </summary>
        [Test]
        public void ReadByName()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("5")));
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("6")));

            var tvshows = this.tvshowsRepository.ReadByName("Name");
            Assert.True(tvshows.Count >= 2);
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowsRepository.ReadAll());
        }

        /// <summary>
        /// The add season.
        /// </summary>
        [Test]
        public void AddSeason()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("7")));
            Assert.True(this.tvshowsRepository.AddSeason("7", Utils.CreateSeason("7", 3).GetSynopsis()));
            Assert.True(this.tvshowsRepository.Read("7").Seasons.Any(synopsis => synopsis.Id.SeasonNumber == 3));
        }

        /// <summary>
        /// The add season.
        /// </summary>
        [Test]
        public void RemoveSeason()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("7")));
            Assert.True(this.tvshowsRepository.RemoveSeason("7", Utils.CreateSeason("7", 2).GetSynopsis()));
            Assert.False(this.tvshowsRepository.Read("7").Seasons.Any(synopsis => synopsis.Id.SeasonNumber == 2));
        }

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Utils.CleanDatabase();
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Utils.CleanDatabase();
        }
    }
}
