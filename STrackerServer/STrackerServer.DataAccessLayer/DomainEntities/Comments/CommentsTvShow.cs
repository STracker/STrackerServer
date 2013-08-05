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
        /// Initializes a new instance of the <see cref="CommentsTvShow"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public CommentsTvShow(string id) : base(id)
        {
        }
    }
}