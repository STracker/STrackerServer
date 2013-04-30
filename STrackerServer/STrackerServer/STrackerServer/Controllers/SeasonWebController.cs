// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonWebController.cs" company="STracker">
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
    using System.Web.Mvc;
    using BusinessLayer.Core;
    using Models.Season;

    /// <summary>
    /// The season web controller.
    /// </summary>
    public class SeasonWebController : Controller
    {
        /// <summary>
        /// The television show operations.
        /// </summary>
        private readonly ITvShowsOperations showOps;

        /// <summary>
        /// The season operations.
        /// </summary>
        private readonly ISeasonsOperations seasonOps;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonWebController"/> class.
        /// </summary>
        /// <param name="showOps">
        /// The show ops.
        /// </param>
        /// <param name="seasonOps">
        /// The season ops.
        /// </param>
        public SeasonWebController(ITvShowsOperations showOps, ISeasonsOperations seasonOps)
        {
            this.showOps = showOps;
            this.seasonOps = seasonOps;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonId">
        /// The season id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId, int seasonId)
        {
            var season = this.seasonOps.Read(new Tuple<string, int>(tvshowId, seasonId));

            if (season == null)
            {
                return this.View("Error");
            }

            var seasonWeb = new SeasonWeb
                                {
                                    TvShowId = tvshowId,
                                    Artwork = this.showOps.Read(tvshowId).Artworks.First().ImageUrl,
                                    EpisodeList = season.EpisodeSynopses.OrderBy(ep => ep.EpisodeNumber)
                                };

            return this.View(seasonWeb);
        }
    }
}