// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for seasons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutSeasons_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons API Controller.
    /// </summary>
    public class SeasonsController : BaseController
    {
        /// <summary>
        /// Operations object.
        /// </summary>
        private readonly ISeasonsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsController"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The seasons operations.
        /// </param>
        public SeasonsController(ISeasonsOperations seasonsOperations)
        {
            this.operations = seasonsOperations;
        }

        /// <summary>
        /// Get one season by is number from one television show by is id.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <param name="number">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [Caching]
        public HttpResponseMessage Get(string tvshowId, int number)
        {
            return this.BaseGetForEntities<Season, Season.SeasonId>(this.operations.Read(new Season.SeasonId { TvShowId = tvshowId, SeasonNumber = number }));
        }

        /// <summary>
        /// Get all seasons from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string tvshowId)
        {
            return this.BaseGet(this.operations.GetAllFromOneTvShow(tvshowId));
        }
    }
}