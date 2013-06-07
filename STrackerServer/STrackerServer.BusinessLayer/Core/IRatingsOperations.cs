// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Interface that defines the contract of operations over comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Core
{
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The RatingsOperations interface.
    /// </summary>
    /// <typeparam name="TR">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public interface IRatingsOperations<out TR, in TK> where TR : BaseRatings<TK>
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

        /// <summary>
        /// The get all ratings.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TR"/>.
        /// </returns>
        TR GetAllRatings(TK key);
    }
}
