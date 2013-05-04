﻿// --------------------------------------------------------------------------------------------------------------------
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
    using BusinessLayer.Core;
    using Models.Season;

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
        /// Initializes a new instance of the <see cref="SeasonsWebController"/> class.
        /// </summary>
        /// <param name="seasonOps">
        /// The season ops.
        /// </param>
        public SeasonsWebController(ISeasonsOperations seasonOps)
        {
            this.seasonOps = seasonOps;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="number">
        /// The season id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId, int number)
        {
            var season = this.seasonOps.Read(new Tuple<string, int>(tvshowId, number));

            if (season == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var model = new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.EpisodeSynopses.OrderBy(ep => ep.EpisodeNumber),
                SeasonNumber = season.SeasonNumber
            };

            return this.View(model);
        }
    }
}