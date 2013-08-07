// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RatingsEpisode.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of episode show ratings domain entity. 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Ratings
{
    using System;

    /// <summary>
    /// The episode ratings.
    /// </summary>
    public class RatingsEpisode : RatingsBase<Tuple<string, int, int>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsEpisode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public RatingsEpisode(Tuple<string, int, int> id) : base(id)
        {
            this.TvShowId = id.Item1;
            this.SeasonNumber = id.Item2;
            this.EpisodeNumber = id.Item3;
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