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

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.Models.Genre;

    /// <summary>
    /// The genre web controller.
    /// </summary>
    public class GenresController : Controller
    {
        /// <summary>
        /// The max television shows returned.
        /// </summary>
        public const int MaxTvShows = 6; 

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
                return this.View("NotFound");
            }

            return this.View(new GenreView { GenreName = genre.Id, TvShows = genre.Tvshows });
        }

        /// <summary>
        /// The get television shows with the most percentage of genres.
        /// </summary>
        /// <param name="genres">
        /// The genres.
        /// </param>
        /// <param name="excludeTvShow">
        /// The exclude television show.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult GetSimilar(string[] genres, string excludeTvShow)
        {
            var tvshows = this.genreOperations.GetTvShows(genres, excludeTvShow, MaxTvShows);

            if (tvshows.Count == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }

            return this.View(new SimilarTvShowsView { TvShows = tvshows });
        }
    }
}