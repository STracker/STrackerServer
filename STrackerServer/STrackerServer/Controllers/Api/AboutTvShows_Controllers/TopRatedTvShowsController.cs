// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopRatedTvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for top rated television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutTvShows_Controllers
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;

    /// <summary>
    /// The top rated television shows controller.
    /// </summary>
    public class TopRatedTvShowsController : BaseController
    {
        /// <summary>
        /// The max top rated television shows indicated in the configuration file.
        /// </summary>
        private static readonly int MaxTopRatedTvShows = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTopRatedTvShows"]);

        /// <summary>
        /// Operations object.
        /// </summary>
        private readonly ITvShowsRatingsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopRatedTvShowsController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public TopRatedTvShowsController(ITvShowsRatingsOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// Get the top rated television shows in STracker.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.operations.GetTopRated(MaxTopRatedTvShows));
        }
    }
}