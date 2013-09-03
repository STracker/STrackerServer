// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoriesSetup.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The test setup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;

    /// <summary>
    /// The test setup.
    /// </summary>
    [SetUpFixture]
    public class RepositoriesSetup
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
        /// Initializes a new instance of the <see cref="RepositoriesSetup"/> class.
        /// </summary>
        public RepositoriesSetup()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            this.genresRepository = kernel.Get<IGenresRepository>();
        }

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Utils.CleanDatabase();

            this.genresRepository.Create(Utils.CreateGenre("Genre1"));
            this.genresRepository.Create(Utils.CreateGenre("Genre2"));

            this.TvShowRepositorySetup();
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Utils.CleanDatabase();
        }

        /// <summary>
        /// The television show repository setup.
        /// </summary>
        private void TvShowRepositorySetup()
        {
            // Create fail
            this.tvshowsRepository.Create(Utils.CreateTvShow("2"));

            // Read
            this.tvshowsRepository.Create(Utils.CreateTvShow("3"));

            // Update
            this.tvshowsRepository.Create(Utils.CreateTvShow("8"));

            // Delete
            this.tvshowsRepository.Create(Utils.CreateTvShow("4"));

            // ReadByName
            this.tvshowsRepository.Create(Utils.CreateTvShow("5"));
            this.tvshowsRepository.Create(Utils.CreateTvShow("6"));

            // Add and remove season
            this.tvshowsRepository.Create(Utils.CreateTvShow("7"));
        }
    }
}
