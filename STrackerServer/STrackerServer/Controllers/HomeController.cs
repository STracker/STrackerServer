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
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core;
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
        private readonly int maxTopRated;

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

            this.maxTopRated = int.Parse(ConfigurationManager.AppSettings["MaxTopRatedTvShows"]);
        }

        /// <summary>
        /// The main page.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly",
            Justification = "Reviewed. Suppression is OK here.")][HttpGet]
        public ActionResult Index()
        {
            var view = new HomeView
            {
                Genres = this.genresOperations.ReadAllSynopsis().OrderBy(synopsis => synopsis.Name).ToList(),
                TopRated = this.tvshowsRatingsOperations.GetTopRated(maxTopRated),
                NewEpisodes = this.tvshowNewEpisodesOperations.GetNewEpisodes(DateTime.Now.AddDays(3))
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