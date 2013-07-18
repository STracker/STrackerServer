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
    using System.Linq;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.Models.Home;

    /// <summary>
    /// The home web controller.
    /// </summary>
    public class HomeWebController : Controller
    {
        /// <summary>
        /// The max top rated.
        /// </summary>
        private const int MaxTopRated = 5;

        /// <summary>
        /// The genres operations.
        /// </summary>
        private readonly IGenresOperations genresOperations;

        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsOperations;

        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly IEpisodesOperations episodesOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeWebController"/> class.
        /// </summary>
        /// <param name="genresOperations">
        /// The genres operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations
        ///  </param>
        /// <param name="episodesOperations">
        /// The episodes Operations.
        /// </param>
        public HomeWebController(IGenresOperations genresOperations, ITvShowsOperations tvshowsOperations, IEpisodesOperations episodesOperations)
        {
            this.genresOperations = genresOperations;
            this.tvshowsOperations = tvshowsOperations;
            this.episodesOperations = episodesOperations;
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
                    Genres = this.genresOperations.GetAll().OrderBy(synopsis => synopsis.Id),
                    TopRated = this.tvshowsOperations.GetTopRated(MaxTopRated),
                    NewEpisodes = this.episodesOperations.GetNewestEpisodes(DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")).Select(episodes => new NewestEpisodesView
                        {
                            TvShowId = episodes.Key,
                            TvShowName = this.tvshowsOperations.Read(episodes.Key).Name,
                            Episodes = episodes.Episodes
                        })
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
