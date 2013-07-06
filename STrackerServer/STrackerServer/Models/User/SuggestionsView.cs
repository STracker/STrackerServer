// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionsView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The suggestions view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The suggestions view.
    /// </summary>
    public class SuggestionsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestionsView"/> class.
        /// </summary>
        public SuggestionsView()
        {
            this.Suggestions = new Dictionary<string, SuggestionView>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the picture url.
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets the suggestions.
        /// </summary>
        public IDictionary<string, SuggestionView> Suggestions { get; set; }
    }
}