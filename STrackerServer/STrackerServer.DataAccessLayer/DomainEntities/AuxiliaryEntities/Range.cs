// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Range.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the Range type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    /// <summary>
    /// The range of a request.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// Gets or sets the start of the range.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the end of the range.
        /// </summary>
        public int End { get; set; }
    }
}
