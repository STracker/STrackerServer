// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The seasons repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
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
        /// Test create.
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
    }
}
