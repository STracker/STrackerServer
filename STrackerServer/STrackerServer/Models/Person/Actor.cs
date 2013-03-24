// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Actor.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.Models.Person
{
    using STrackerServer.Models.Utils;

    /// <summary>
    /// The actor.
    /// </summary>
    public class Actor : Person
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public CharacterRoles Role { get; set; }

        /// <summary>
        /// Gets or sets the character name.
        /// </summary>
        public string CharacterName { get; set; }
    }
}