// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;

    /// <summary>
    /// Television shows API controller.
    /// </summary>
    public class TvShowsController : BaseController
    {
        /// <summary>
        /// The max top rated television shows.
        /// </summary>
        private static readonly int MaxTopRatedTvShows = Convert.ToInt32(ConfigurationManager.AppSettings["MaxTopRatedTvShows"]);

        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ITvShowsOperations operations;

        /// <summary>
        /// The ratings operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations ratingsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsController"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        /// <param name="ratingsOperations">
        /// The ratings Operations.
        /// </param>
        public TvShowsController(ITvShowsOperations tvshowsOperations, ITvShowsRatingsOperations ratingsOperations)
        {
            this.operations = tvshowsOperations;
            this.ratingsOperations = ratingsOperations;
        }

        /// <summary>
        /// Get information from the television show with id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            return this.BaseGet(this.operations.Read(id));
        }

        /// <summary>
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public HttpResponseMessage GetByName(string name)
        {
            return this.BaseGet(this.operations.ReadByName(name));
        }

        /// <summary>
        /// The get top rated.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        public HttpResponseMessage GetTopRated()
        {
            return this.BaseGet(this.ratingsOperations.GetTopRated(MaxTopRatedTvShows));
        }
    }
}