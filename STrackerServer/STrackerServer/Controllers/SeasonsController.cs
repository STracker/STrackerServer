// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Season Web Controller
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
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
    public class SeasonsController : Controller
    {
        /// <summary>
        /// The season operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerServer.Controllers.SeasonsController"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The season ops.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows ops.
        /// </param>
        public SeasonsController(ISeasonsOperations seasonsOperations, ITvShowsOperations tvshowsOperations)
        {
            this.seasonsOperations = seasonsOperations;
            this.tvshowsOperations = tvshowsOperations;
        }

        /// <summary>
        /// The television show season view.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index(string tvshowId, int seasonNumber)
        {
            var key = new Season.SeasonId { TvShowId = tvshowId, SeasonNumber = seasonNumber };

            var season = this.seasonsOperations.Read(key);

            if (season == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            var tvshow = this.tvshowsOperations.Read(tvshowId);

            return this.View(new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.Episodes.OrderBy(ep => ep.Id.EpisodeNumber),
                SeasonNumber = season.Id.SeasonNumber,
                Poster = tvshow.Poster,
                TvShowName = tvshow.Name
            });
        }
    }
}