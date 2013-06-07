// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Comment.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Implementation of comment object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    /// <summary>
    /// The comment.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the comment body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        public string Timestamp { get; set; }
    }
}