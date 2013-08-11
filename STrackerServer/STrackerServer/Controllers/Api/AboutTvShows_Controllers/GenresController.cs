// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for genres.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutTvShows_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;

    /// <summary>
    /// The genres controller.
    /// </summary>
    public class GenresController : BaseController
    {
        /// <summary>
        /// Operations object.
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
        /// Get one genre by is name (identifier).
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
            return this.BaseGet(genre);
        }

        /// <summary>
        /// Get all the genres available in the STracker.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return this.BaseGet(this.operations.ReadAllSynopsis());
        }
    }
}