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
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.Models.Home;

    /// <summary>
    /// The home web controller.
    /// </summary>
    public class HomeWebController : Controller
    {
        /// <summary>
        /// The genres operations.
        /// </summary>
        private readonly IGenresOperations genresOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeWebController"/> class.
        /// </summary>
        /// <param name="genresOperations">
        /// The genres operations.
        /// </param>
        public HomeWebController(IGenresOperations genresOperations)
        {
            this.genresOperations = genresOperations;
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
            var view = new HomeView { Genres = this.genresOperations.GetAll() };

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
