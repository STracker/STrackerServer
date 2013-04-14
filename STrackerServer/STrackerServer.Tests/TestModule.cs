// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModule.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Ninject module for tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests
{
    using Ninject.Modules;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Facades;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.Tests.CustomRepositories;

    /// <summary>
    /// The NINJECT test module.
    /// </summary>
    public class TestModule : NinjectModule
    {
        /// <summary>
        /// The load.
        /// </summary>
        public override void Load()
        {
            this.Bind<ITvShowsOperations>().To<TvShowsOperations>();
            this.Bind<ITvShowsRepository>().To<CustomTvShowsRepository>();
        }
    }
}