// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Season Web Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Models.Season;

    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The season web controller.
    /// </summary>
    public class SeasonsWebController : Controller
    {
        /// <summary>
        /// The season operations.
        /// </summary>
        private readonly ISeasonsOperations seasonOps;

        /// <summary>
        /// The television shows ops.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOps;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsWebController"/> class.
        /// </summary>
        /// <param name="seasonOps">
        /// The season ops.
        /// </param>
        /// <param name="tvshowsOps">
        /// The television shows ops.
        /// </param>
        public SeasonsWebController(ISeasonsOperations seasonOps, ITvShowsOperations tvshowsOps)
        {
            this.seasonOps = seasonOps;
            this.tvshowsOps = tvshowsOps;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season Number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId, int seasonNumber)
        {
            var season = this.seasonOps.Read(new Season.SeasonKey { TvshowId = tvshowId, SeasonNumber = seasonNumber });

            if (season == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOps.Read(tvshowId);

            return this.View(new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.EpisodeSynopsis.OrderBy(ep => ep.Id.EpisodeNumber),
                SeasonNumber = season.Id.SeasonNumber,
                Poster = tvshow.Poster,
                TvShowName = tvshow.Name   
            });
        }
    }
}