// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of person object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    /// <summary>
    /// Person domain entity.
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
        public string Photo { get; set; }
    }
}