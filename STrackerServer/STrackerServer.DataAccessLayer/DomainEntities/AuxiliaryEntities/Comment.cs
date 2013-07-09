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
    using System;

    /// <summary>
    /// The comment.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User.UserSynopsis User { get; set; }

        /// <summary>
        /// Gets or sets the comment body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the Comment Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }
    }
}