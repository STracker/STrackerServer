// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Core
{
    /// <summary>
    /// Entity interface.
    /// </summary>
    /// <typeparam name="TK">
    /// Type of the entity id.
    /// </typeparam>
    public interface IEntity<TK>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        TK Id { get; set; }
    }
}