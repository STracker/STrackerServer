// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for genres.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;

    /// <summary>
    /// The genres controller.
    /// </summary>
    public class GenresController : BaseController
    {
        /// <summary>
        /// The operations.
        /// </summary>
        private readonly IGenresOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenresController"/> class.
        /// </summary>
        /// <param name="operations">
        /// The operations.
        /// </param>
        public GenresController(IGenresOperations operations)
        {
            this.operations = operations;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            return this.BaseGet(this.operations.GetAll());
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            var genre = this.operations.Read(id);

            if (genre == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return this.BaseGet(genre.TvshowsSynopses);
        }
    }
}