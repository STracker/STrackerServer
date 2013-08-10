// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;

    /// <summary>
    /// Episodes API controller.
    /// </summary>
    public class EpisodesController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IEpisodesOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesController"/> class.
        /// </summary>
        /// <param name="episodesOperations">
        /// The episodes operations.
        /// </param>
        public EpisodesController(IEpisodesOperations episodesOperations)
        {
            this.operations = episodesOperations;
        }

        /// <summary>
        /// Get information from the episode from one season from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string tvshowId, int seasonNumber, int number)
        {
            //return this.BaseGet(this.operations.Read(new Tuple<string, int, int>(tvshowId, seasonNumber, number)));
            return null;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage GetAll(string tvshowId, int seasonNumber)
        {
            //return this.BaseGet(this.operations.GetAllFromOneSeason(tvshowId, seasonNumber));
            return null;
        }
    }
}