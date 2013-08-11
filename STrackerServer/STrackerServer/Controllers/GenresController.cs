﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genre web controller.
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
    public class GenresController : Controller
    {
        /// <summary>
        /// The genre operations.
        /// </summary>
        private readonly IGenresOperations genreOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenresController"/> class. 
        /// </summary>
        /// <param name="genreOperations">
        /// The genre operations.
        /// </param>
        public GenresController(IGenresOperations genreOperations)
        {
            this.genreOperations = genreOperations;
        }

        /// <summary>
        /// Represents all the television shows in the specific genre.
        /// </summary>
        /// <param name="id">
        /// The genre name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index(string id)
        {
            var genre = this.genreOperations.Read(id);

            if (genre == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.View("Error", Response.StatusCode);
            }

            return this.View(new GenreView { GenreName = genre.Id, TvShows = genre.Tvshows });
        }
    }
}