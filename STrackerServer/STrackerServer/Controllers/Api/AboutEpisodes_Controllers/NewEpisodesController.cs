// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewEpisodesController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api controller for new episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutEpisodes_Controllers
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The new episodes controller.
    /// </summary>
    public class NewEpisodesController : BaseController
    {
        /// <summary>
        /// The new episodes operations.
        /// </summary>
        private readonly ITvShowNewEpisodesOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEpisodesController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public NewEpisodesController(ITvShowNewEpisodesOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// Get new episodes from one television show until date passed in parameters,
        /// if the date in parameters is null, return all episodes.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [Caching]
        public HttpResponseMessage Get(string tvshowId, string date)
        {
            DateTime dateTime;
            return this.BaseGetForEntities<NewTvShowEpisodes, string>(this.operations.GetNewEpisodes(tvshowId, DateTime.TryParse(date, out dateTime) ? dateTime : (DateTime?)null));
        }
    }
}