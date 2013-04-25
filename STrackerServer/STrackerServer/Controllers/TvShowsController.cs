// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Television shows API controller.
    /// </summary>
    public class TvShowsController : BaseController<TvShow, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsController"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        public TvShowsController(ITvShowsOperations tvshowsOperations)
            : base(tvshowsOperations)
        {
        }

        /// <summary>
        /// Get information from the television show with id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        [HttpGet]
        public TvShow Get(string id)
        {
            return this.Get(this.Operations.Read(id));
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
        public TvShow GetByName(string name)
        {
            return this.Get(((ITvShowsOperations)this.Operations).ReadByName(name));
        }
    }
}
