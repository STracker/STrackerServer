// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentsTvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.Comments
{
    /// <summary>
    /// The television show comments.
    /// </summary>
    public class CommentsTvShow : CommentsBase<string>
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }
    }
}