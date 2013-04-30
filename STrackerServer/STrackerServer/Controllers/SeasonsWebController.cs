// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Season Web Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using STrackerServer.DataAccessLayer.DomainEntities;

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
    public class SeasonsWebController : BaseWebController
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
        /// Initializes a new instance of the <see cref="SeasonsWebController"/> class.
        /// </summary>
        /// <param name="showOps">
        /// The show ops.
        /// </param>
        /// <param name="seasonOps">
        /// The season ops.
        /// </param>
        public SeasonsWebController(ITvShowsOperations showOps, ISeasonsOperations seasonOps)
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
        /// <param name="number">
        /// The season id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string tvshowId, int number)
        {
            OperationResultState state;
            Season season = this.seasonOps.TryRead(new Tuple<string, int>(tvshowId, number), out state);

            if (season == null)
            {
                return this.GetView(state);
            }

            SeasonView model = new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.EpisodeSynopses.OrderBy(ep => ep.EpisodeNumber),
                SeasonNumber = season.SeasonNumber
            };

            return this.View(model);
        }
    }
}