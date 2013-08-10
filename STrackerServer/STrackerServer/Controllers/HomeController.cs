// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.Models.Home;

    /// <summary>
    /// The home web controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The max top rated.
        /// </summary>
        private const int MaxTopRated = 8;

        /// <summary>
        /// The genres operations.
        /// </summary>
        private readonly IGenresOperations genresOperations;

        /// <summary>
        /// The new television show episodes operations.
        /// </summary>
        private readonly ITvShowNewEpisodesOperations tvshowNewEpisodesOperations;

        /// <summary>
        /// The television shows ratings operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations tvshowsRatingsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="genresOperations">
        /// The genres operations.
        /// </param>
        /// <param name="tvshowsRatingsOperations">
        /// The television shows ratings operations.
        /// </param>
        /// <param name="tvshowNewEpisodesOperations">
        /// The new television show episodes operations.
        /// </param>
        public HomeController(IGenresOperations genresOperations, ITvShowsRatingsOperations tvshowsRatingsOperations, ITvShowNewEpisodesOperations tvshowNewEpisodesOperations)
        {
            this.genresOperations = genresOperations;
            this.tvshowNewEpisodesOperations = tvshowNewEpisodesOperations;
            this.tvshowsRatingsOperations = tvshowsRatingsOperations;
        }

        /// <summary>
        /// The main page.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var view = new HomeView
            {
                Genres = this.genresOperations.ReadAllSynopsis().OrderBy(synopsis => synopsis),
                TopRated = this.tvshowsRatingsOperations.GetTopRated(MaxTopRated),
                NewEpisodes = this.tvshowNewEpisodesOperations.GetNewEpisodes(DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"))
            };

            return this.View(view);
        }

        /// <summary>
        /// API information.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Api()
        {
            return this.View();
        }
    }
}