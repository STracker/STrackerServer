// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModuleForMongoRepositories.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Ninject module for mongo repositories tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.NinjectModules
{
    using MongoDB.Driver;

    using Ninject.Modules;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.Repository.MongoDB.Core;

    /// <summary>
    /// The NINJECT test module.
    /// </summary>
    public class TestModuleForMongoRepositories : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            this.Bind<MongoUrl>().ToSelf().InSingletonScope().WithConstructorArgument("url", "mongodb://STrackerServer:stracker_admin@ds057857.mongolab.com:57857/appharbor_1e0e7e03-49be-42e9-b656-8f3c64ab59f0");
            this.Bind<MongoClient>().ToSelf().InSingletonScope();

            this.Bind<ITvShowsRepository>().To<TvShowsRepository>();
            this.Bind<ISeasonsRepository>().To<SeasonsRepository>();
            this.Bind<IEpisodesRepository>().To<EpisodesRepository>();
        }
    }
}