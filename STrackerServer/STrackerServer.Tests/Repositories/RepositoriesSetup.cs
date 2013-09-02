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
    using System.Linq;

    using MongoDB.Driver;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;

    /// <summary>
    /// The test setup.
    /// </summary>
    [SetUpFixture]
    public class RepositoriesSetup
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly MongoDatabase database;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoriesSetup"/> class.
        /// </summary>
        public RepositoriesSetup()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.database = kernel.Get<MongoClient>().GetServer().GetDatabase(Utils.DatabaseName);

            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
        }

        /// <summary>
        /// The setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.CleanDatabase();

            // CreateFail
            this.tvshowsRepository.Create(Utils.CreateTvShowDummy("2"));

            // Read
            this.tvshowsRepository.Create(Utils.CreateTvShowDummy("3"));

            // Update
            this.tvshowsRepository.Create(Utils.CreateTvShowDummy("4"));

            // Delete
            this.tvshowsRepository.Create(Utils.CreateTvShowDummy("5"));
        }

        /// <summary>
        /// The tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.CleanDatabase();
        }

        /// <summary>
        /// Clean database.
        /// </summary>
        public void CleanDatabase()
        {
            foreach (var collectionName in this.database.GetCollectionNames().Where(name => !name.Equals("system.indexes") && !name.Equals("system.users")))
            {
                this.database.DropCollection(collectionName);
            }
        }
    }
}
