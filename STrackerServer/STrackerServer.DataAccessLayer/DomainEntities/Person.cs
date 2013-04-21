// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of person domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Person domain entity.
    /// </summary>
    public class Person : IEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public Person(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

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