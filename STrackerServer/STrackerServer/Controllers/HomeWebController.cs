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
    using System.Linq;
    using System.Web.Mvc;

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
        /// Initializes a new instance of the <see cref="HomeWebController"/> class.
        /// </summary>
        /// <param name="genresOperations">
        /// The genres operations.
        /// </param>
        /// <param name="tvshowsOperations">
        /// The television shows operations
        ///  </param>
        public HomeWebController(IGenresOperations genresOperations, ITvShowsOperations tvshowsOperations)
        {
            this.genresOperations = genresOperations;
            this.tvshowsOperations = tvshowsOperations;
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
                    TopRated = this.tvshowsOperations.GetTopRated(MaxTopRated)
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
