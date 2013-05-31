// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeComments.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System;

    /// <summary>
    /// The episode comments.
    /// </summary>
    public class EpisodeComments : Container<Tuple<string, int, int>, Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public EpisodeComments(Tuple<string, int, int> key)
            : base(key, "Comments")
        {
            this.TvShowId = key.Item1;
            this.SeasonNumber = key.Item2;
            this.EpisodeNumber = key.Item3;
        }

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
