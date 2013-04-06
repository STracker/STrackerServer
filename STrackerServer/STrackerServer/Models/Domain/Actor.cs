// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Actor.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.Domain
{
    /// <summary>
    /// The actor domain entity.
    /// </summary>
    public class Actor : Person
    {
        /// <summary>
        /// Gets or sets the character name.
        /// </summary>
        public string CharacterName { get; set; }
    }
}