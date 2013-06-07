// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsEpisode.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Comments
{
    using System;

    /// <summary>
    /// The episode comments.
    /// </summary>
    public class CommentsEpisode : CommentsBase<Tuple<string, int, int>>
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the season number.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Gets or sets the episode number.
        /// </summary>
        public int EpisodeNumber { get; set; }
    }
}
