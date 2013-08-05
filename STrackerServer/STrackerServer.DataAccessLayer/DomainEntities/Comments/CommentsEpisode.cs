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
    /// <summary>
    /// The episode comments.
    /// </summary>
    public class CommentsEpisode : CommentsBase<Episode.EpisodeKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsEpisode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public CommentsEpisode(Episode.EpisodeKey id) : base(id)
        {
        }
    }
}