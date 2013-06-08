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
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.Models.TvShow;

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
        /// The users operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsWebController"/> class.
        /// </summary>
        /// <param name="seasonOps">
        /// The season ops.
        /// </param>
        /// <param name="tvshowsOps">
        /// The television shows ops.
        /// </param>
        /// <param name="usersOperations">
        /// The users Operations.
        /// </param>
        public SeasonsWebController(ISeasonsOperations seasonOps, ITvShowsOperations tvshowsOps, IUsersOperations usersOperations)
        {
            this.seasonOps = seasonOps;
            this.tvshowsOps = tvshowsOps;
            this.usersOperations = usersOperations;
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

            var tvshow = this.tvshowsOps.Read(tvshowId);

            var model = new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.EpisodeSynopses.OrderBy(ep => ep.EpisodeNumber),
                SeasonNumber = season.SeasonNumber,
                Poster = tvshow.Poster.ImageUrl,
                Options =
                        TvShowOptions.Create(
                            tvshow, this.usersOperations.Read(User.Identity.Name), Url.Action("Show", new { tvshowId, number}))
            };

            return this.View(model);
        }
    }
}