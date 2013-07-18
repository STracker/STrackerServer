// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewestEpisodesView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The newest episodes view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Home
{
    using System.Collections.Generic;

    /// <summary>
    /// The newest episodes view.
    /// </summary>
    public class NewestEpisodesView
    {
        /// <summary>
        /// Gets or sets the television show.
        /// </summary>
        public DataAccessLayer.DomainEntities.TvShow.TvShowSynopsis TvShow { get; set; }

        /// <summary>
        /// Gets or sets the episodes.
        /// </summary>
        public List<DataAccessLayer.DomainEntities.Episode.EpisodeSynopsis> Episodes { get; set; } 
    }
}