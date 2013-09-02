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
    using Ninject;

    using NUnit.Framework;

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
        /// Initializes a new instance of the <see cref="TvShowsRepositoryTests"/> class.
        /// </summary>
        public TvShowsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());

            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var tvshow = Utils.CreateTvShowDummy("1");
            Assert.True(this.tvshowsRepository.Create(tvshow));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            Assert.False(this.tvshowsRepository.Create(Utils.CreateTvShowDummy("2")));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
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
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.NotNull(this.tvshowsRepository.Read("5"));
            Assert.True(this.tvshowsRepository.Delete("5"));
            Assert.Null(this.tvshowsRepository.Read("5"));
        }
    }
}
