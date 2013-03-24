// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Person
{
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The person.
    /// </summary>
    public class Person
    {
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