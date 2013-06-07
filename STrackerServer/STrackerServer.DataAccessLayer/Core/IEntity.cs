// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of the domain entities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    /// <summary>
    /// IEntity interface.
    /// </summary>
    /// <typeparam name="TK">
    /// Type of entity's id.
    /// </typeparam>
    public interface IEntity<TK>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        TK Key { get; set; }
    }
}