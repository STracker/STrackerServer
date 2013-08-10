// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the Extensions of IEnumerable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Collections.Generic;
    using System.Linq;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Apply the range of the element in a specific enumerable.
        /// </summary>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <typeparam name="T">
        /// Enumerable elements Type
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static ICollection<T> ApplyRange<T>(this ICollection<T> enumerable, Range range)
        {
            return range != null && range.End > range.Start ? enumerable.Skip(range.Start).Take(range.End - range.Start).ToList() : enumerable;
        }
    }
}
