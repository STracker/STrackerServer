// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeWebController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Account Controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Models.Home;

    /// <summary>
    /// The home web controller.
    /// </summary>
    public class HomeWebController : Controller
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
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The new episodes operations.
        /// </summary>
        private readonly INewEpisodesOperations newEpisodesOperations;

        /// <summary>
        /// The television shows ratings operations.
        /// </summary>
        private readonly ITvShowsRatingsOperations tvshowsRatingsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeWebController"/> class.
        /// </summary>
        /// <param name="genresOperations">
        /// The genres operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations
        ///  </param>
        /// <param name="newEpisodesOperations">
        /// The new Episodes Operations.
        /// </param>
        /// <param name="tvshowsRatingsOperations">
        /// The television shows Ratings Operations.
        /// </param>
        public HomeWebController(IGenresOperations genresOperations, ITvShowsOperations tvshowsOperations, INewEpisodesOperations newEpisodesOperations, ITvShowsRatingsOperations tvshowsRatingsOperations)
        {
            this.genresOperations = genresOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.newEpisodesOperations = newEpisodesOperations;
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
                    Genres = this.genresOperations.GetAll().OrderBy(g => g.Id).ToList(),
                    TopRated = this.tvshowsRatingsOperations.GetTopRated(MaxTopRated).ToList(),
                    NewEpisodes = this.newEpisodesOperations.GetNewEpisodes(DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")).ToList()
                };

            return this.View(view);
        }

        /// <summary>
        /// The contact.
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
