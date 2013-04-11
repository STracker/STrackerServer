// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of person domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Models
{
    using STrackerServerDatabase.Core;

    /// <summary>
    /// Person domain entity.
    /// </summary>
    public class Person : IEntity<string>
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
        /// Gets or sets the photo.
        /// </summary>
        public Artwork Photo { get; set; }
    }
}