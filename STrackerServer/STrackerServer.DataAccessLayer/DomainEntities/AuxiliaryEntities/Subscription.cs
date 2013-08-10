// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Subscription.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of subscription object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// The subscription.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        public Subscription()
        {
            this.EpisodesWatched = new List<Episode.EpisodeSynopsis>();
        }

        /// <summary>
        /// Gets or sets the television show.
        /// </summary>
        public TvShow.TvShowSynopsis TvShow { get; set; }

        /// <summary>
        /// Gets or sets the episodes watched.
        /// </summary>
        public List<Episode.EpisodeSynopsis> EpisodesWatched { get; set; }
    }
}
