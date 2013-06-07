// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeRatings.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of episode show ratings domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System;

    /// <summary>
    /// The episode ratings.
    /// </summary>
    public class EpisodeRatings : BaseRatings<Tuple<string, int, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeRatings"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public EpisodeRatings(Tuple<string, int, int> key) : base(key)
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