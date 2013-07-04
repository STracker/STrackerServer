﻿// --------------------------------------------------------------------------------------------------------------------
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

            var view = new GenreView { GenreName = genreModel.Key, TvShows = genreModel.TvshowsSynopses };
            return this.View(view);
        }
    }
}