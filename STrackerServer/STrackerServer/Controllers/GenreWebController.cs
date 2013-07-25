// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenreWebController.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the GenreWebController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Net;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.Models.Genre;

    /// <summary>
    /// The genre web controller.
    /// </summary>
    public class GenreWebController : Controller
    {
        /// <summary>
        /// The genre operations.
        /// </summary>
        private readonly IGenresOperations genreOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreWebController"/> class.
        /// </summary>
        /// <param name="genreOperations">
        /// The genre operations.
        /// </param>
        public GenreWebController(IGenresOperations genreOperations)
        {
            this.genreOperations = genreOperations;
        }

        /// <summary>
        /// The show.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Show(string name)
        {
            var genreModel = this.genreOperations.Read(name);

            if (genreModel == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new GenreView { GenreName = genreModel.Key, TvShows = genreModel.TvshowsSynopses });
        }

        /// <summary>
        /// The get television shows.
        /// </summary>
        /// <param name="genres">
        /// The genres.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult GetTvShows(string[] genres, int max)
        {
            if (genres == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }

            return this.Json(this.genreOperations.GetTvShows(genres, max), JsonRequestBehavior.AllowGet);
        }
    }
}
