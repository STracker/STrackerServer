// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Media.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of media domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Models
{
    using System.Collections.Generic;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// Media domain entity.
    /// </summary>
    public class Media : IEntity<string>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public List<Genre> Genres { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the artworks.
        /// </summary>
        public List<Artwork> Artworks { get; set; }

        /// <summary>
        /// Gets or sets the actors.
        /// </summary>
        public List<Actor> Actors { get; set; }
    }
}