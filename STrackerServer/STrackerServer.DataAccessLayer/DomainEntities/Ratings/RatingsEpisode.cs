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
    /// <summary>
    /// The episode ratings.
    /// </summary>
    public class RatingsEpisode : RatingsBase<Episode.EpisodeId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RatingsEpisode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public RatingsEpisode(Episode.EpisodeId id) : base(id)
        {
        }
    }
}