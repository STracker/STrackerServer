﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsController.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Controller for seasons.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers
{
    using System;
    using System.Web.Http;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Seasons API Controller.
    /// </summary>
    public class SeasonsController : BaseController<Season, Tuple<string, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsController"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The seasons operations.
        /// </param>
        public SeasonsController(ISeasonsOperations seasonsOperations)
            : base(seasonsOperations)
        {
        }

        /// <summary>
        /// Get information from the season from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        [HttpGet]
        public Season Get(string tvshowId, int number)
        {
            return this.Get(this.Operations.Read(new Tuple<string, int>(tvshowId, number)));
        }
    }
}