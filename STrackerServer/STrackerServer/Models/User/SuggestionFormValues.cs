// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionFormValues.cs" company="STracker">
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
    public class SuggestionFormValues
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
        public string TvShowId { get; set; }
    }
}