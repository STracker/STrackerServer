// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The base ratings repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The RatingsRepository interface.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public interface IRatingsRepository<T, in TK> : IRepository<T, TK> where T : BaseRatings<TK>
    {
        /// <summary>
        /// The add rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddRating(TK key, Rating rating);

        /// <summary>
        /// The remove rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool RemoveRating(TK key, Rating rating);
    }
}
