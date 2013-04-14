// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of domain entities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    /// <summary>
    /// Entities interface.
    /// </summary>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public interface IEntity<TK>
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        TK Key { get; set; }

        /// <summary>
        /// Gets or sets the key in string format.
        /// </summary>
        string Id { get; set; }
    }
}