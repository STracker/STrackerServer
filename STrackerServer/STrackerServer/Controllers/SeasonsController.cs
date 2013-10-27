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

    using STrackerServer.Action_Results;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.BusinessLayer.Core.UsersOperations;

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
        /// The user operations.
        /// </summary>
        private readonly IUsersOperations usersOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerServer.Controllers.SeasonsController"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The season ops.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows ops.
        /// </param>
        /// /// <param name="usersOperations">
        /// The user ops.
        /// </param>
        public SeasonsController(ISeasonsOperations seasonsOperations, ITvShowsOperations tvshowsOperations, IUsersOperations usersOperations)
        {
            this.seasonsOperations = seasonsOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.usersOperations = usersOperations;
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
            var isSubscribed = false;
            var season = this.seasonsOperations.Read(key);

            if (season == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("NotFound");
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = this.usersOperations.Read(User.Identity.Name);
                var subscription = user.Subscriptions.Find(subscription1 => subscription1.TvShow.Id.Equals(tvshowId));

                if (subscription != null)
                {
                    isSubscribed = true;
                }
            }
            var tvshow = this.tvshowsOperations.Read(tvshowId);

            return this.View(new SeasonView
            {
                TvShowId = tvshowId,
                EpisodeList = season.Episodes.OrderBy(ep => ep.Id.EpisodeNumber),
                SeasonNumber = season.Id.SeasonNumber,
                Poster = tvshow.Poster,
                TvShowName = tvshow.Name,
                IsSubscribed = isSubscribed
            });
        }

        /// <summary>
        /// Watch all episodes from a season.
        /// </summary>
        /// <param name="values">
        /// Season information.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [Authorize]
        public ActionResult Watched(SeasonWatched values)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.View("Error", Response.StatusCode);
            }

            var key = new Season.SeasonId { SeasonNumber = values.SeasonNumber, TvShowId = values.TvShowId };
            this.usersOperations.AddSeasonWatched(this.User.Identity.Name, key);

            return new SeeOtherResult{ Url = Url.Action("Index","Seasons", new { tvshowId = values.TvShowId, seasonNumber = values.SeasonNumber })};
        }
    }
}