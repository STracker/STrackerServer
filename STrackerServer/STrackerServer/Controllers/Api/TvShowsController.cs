// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows API controller.
    /// </summary>
    public class TvShowsController : BaseController<TvShow, string>
    {
        /// <summary>
        /// The genres operations.
        /// </summary>
        private readonly IGenresOperations genresOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsController"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        /// <param name="genresOperations">
        /// The genres Operations.
        /// </param>
        public TvShowsController(ITvShowsOperations tvshowsOperations, IGenresOperations genresOperations)
            : base(tvshowsOperations)
        {
            this.genresOperations = genresOperations;
        }

        /// <summary>
        /// Get information from the television show with id.
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
            return this.BaseGet(this.Operations.Read(id));
        }

        /// <summary>
        /// Get one television show by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public HttpResponseMessage GetByName(string name)
        {
            return this.BaseGet(((ITvShowsOperations)this.Operations).ReadByName(name));
        }

        /// <summary>
        /// The get all by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        public HttpResponseMessage GetAllByGenre(string genre)
        {
            return this.BaseGet(this.genresOperations.Read(genre));
        }
    }
}