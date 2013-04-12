// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Actor.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of actor domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    /// <summary>
    /// The actor domain entity.
    /// </summary>
    public class Actor : Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Actor(string id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the character name.
        /// </summary>
        public string CharacterName { get; set; }
    }
}