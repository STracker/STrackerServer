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
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("1")));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason("1", 3)));
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("1")));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason("1", 3)));
            Assert.False(this.seasonsRepository.Create(Utils.CreateSeason("1", 3)));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("1")));
            Assert.True(this.seasonsRepository.Create(Utils.CreateSeason("1", 3)));
            Assert.NotNull(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = "1", SeasonNumber = 3 }));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow("1")));
            Assert.Null(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = "1", SeasonNumber = 3 }));
            Assert.Null(this.seasonsRepository.Read(new Season.SeasonId { TvShowId = "2", SeasonNumber = 3 }));
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
