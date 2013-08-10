// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestFormValues.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The suggestion form values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The suggestion form values.
    /// </summary>
    public class SuggestFormValues
    {
        /// <summary>
        /// Gets or sets the friend id.
        /// </summary>
        [Required]
        public string FriendId { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}