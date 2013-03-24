// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Media.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Media
{
    using System.Collections.Generic;
    using STrackerServer.Models.Person;
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The media.
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

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