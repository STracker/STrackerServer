// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Domain
{
    using STrackerServer.Core;
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The person domain entity.
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