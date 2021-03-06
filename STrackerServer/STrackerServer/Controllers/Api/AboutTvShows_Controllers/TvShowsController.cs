﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Api Controller for television shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AboutTvShows_Controllers
{
    using System.Net.Http;
    using System.Web.Http;

    using STrackerServer.Attributes;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// Television shows API controller.
    /// </summary>
    public class TvShowsController : BaseController
    {
        /// <summary>
        /// Operations object.
        /// </summary>
        private readonly ITvShowsOperations operations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsController"/> class.
        /// </summary>
        /// <param name="tvshowsOperations">
        /// Television shows operations.
        /// </param>
        public TvShowsController(ITvShowsOperations tvshowsOperations)
        {
            this.operations = tvshowsOperations;
        }

        /// <summary>
        /// Get one television show by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        [Caching]
        public HttpResponseMessage Get(string id)
        {
            return this.BaseGetForEntities<TvShow, string>(this.operations.Read(id));
        }

        /// <summary>
        /// Get one or more television shows by name.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage GetByName(string name)
        {
            return this.BaseGet(this.operations.ReadByName(name, new Range()));
        }

        /// <summary>
        /// Get one or more television shows by name. Receives also a range 
        /// for get only one portion of television shows.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="start">
        /// The start value of the range.
        /// </param>
        /// <param name="end">
        /// The end value of the range.
        /// </param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [HttpGet]
        public HttpResponseMessage GetByNameWithRange(string name, int start, int end)
        {
            return this.BaseGet(this.operations.ReadByName(name, new Range { Start = start, End = end }));
        }
    }
}