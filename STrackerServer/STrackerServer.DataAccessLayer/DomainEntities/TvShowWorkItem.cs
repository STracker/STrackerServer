// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowWorkItem.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of television show work item domain entity. 
//  The Key is the imdb id of the television show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// The work item for television shows.
    /// </summary>
    public class TvShowWorkItem : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get;  set; }

        /// <summary>
        /// Gets or sets a value indicating whether work state.
        /// </summary>
        public bool WorkState { get; set; }
    }
}