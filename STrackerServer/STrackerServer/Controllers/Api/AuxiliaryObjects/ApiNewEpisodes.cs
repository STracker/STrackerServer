// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiNewEpisodes.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The api new episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The API new episodes.
    /// </summary>
    public class ApiNewEpisodes
    {
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        public List<NewestEpisodes> List { get; set; } 
    }
}