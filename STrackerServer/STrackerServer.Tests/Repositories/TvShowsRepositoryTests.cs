// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowsRepositoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

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
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.genresRepository.Read("Genre1").Tvshows.Any(synopsis => synopsis.Id.Equals(id)));

            Assert.NotNull(this.tvshowRatingsRepository.Read(id));
            Assert.NotNull(this.tvshowCommentsRepository.Read(id));
            Assert.NotNull(this.tvshowNewEpisodesRepository.Read(id));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.False(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.NotNull(this.tvshowsRepository.Read(id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.tvshowsRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));

            var tvshow = this.tvshowsRepository.Read(id);
            Assert.NotNull(tvshow);

            tvshow.Name = "Name2";

            Assert.True(this.tvshowsRepository.Update(tvshow));

            Assert.True(this.genresRepository.Read("Genre2").Tvshows.Any(synopsis => synopsis.Id.Equals(id) && synopsis.Name.Equals("Name2")));

            Assert.AreEqual(this.tvshowNewEpisodesRepository.Read(id).TvShow.Name, "Name2");
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.tvshowsRepository.Delete(id));
            Assert.Null(this.tvshowsRepository.Read(id));
        }

        /// <summary>
        /// The read by name.
        /// </summary>
        [Test]
        public void ReadByName()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(Utils.CreateId())));
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(Utils.CreateId())));

            var tvshows = this.tvshowsRepository.ReadByName("Name", new Range { Start = 0, End = int.MaxValue });
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
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.tvshowsRepository.AddSeason(id, Utils.CreateSeason(id, 3).GetSynopsis()));
            Assert.True(this.tvshowsRepository.Read(id).Seasons.Any(synopsis => synopsis.Id.SeasonNumber == 3));
        }

        /// <summary>
        /// The add season.
        /// </summary>
        [Test]
        public void RemoveSeason()
        {
            var id = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id)));
            Assert.True(this.tvshowsRepository.RemoveSeason(id, Utils.CreateSeason(id, 2).GetSynopsis()));
            Assert.False(this.tvshowsRepository.Read(id).Seasons.Any(synopsis => synopsis.Id.SeasonNumber == 2));
        }
    }
}