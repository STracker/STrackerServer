// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiAddComment.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  This object encapsulates the necessary values for add a new 
//  comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Controllers.Api.AuxiliaryObjects
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The comment.
    /// </summary>
    public class ApiAddComment
    {
        /// <summary>
        /// Gets or sets the comment body.
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        [Required]
        public string Timestamp { get; set; }
    }
}