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
    public interface ISynopsis
    {
        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        string Uri { get; set; }
    }
}
