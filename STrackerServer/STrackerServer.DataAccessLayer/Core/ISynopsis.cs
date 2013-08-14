// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISynopsis.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of the domain entities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    /// <summary>
    /// EntitySynopsis interface.
    /// </summary>
    /// <typeparam name="T">
    /// The Id type
    /// </typeparam>
    public interface ISynopsis<T>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        T Id { get; set; }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        string Uri { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }
    }
}
