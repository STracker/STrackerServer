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
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The RatingsOperations interface.
    /// </summary>
    /// <typeparam name="TR">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity key.
    /// </typeparam>
    public interface IRatingsOperations<out TR, in TK> where TR : RatingsBase<TK>
    {
        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TR"/>.
        /// </returns>
        TR Read(TK key);

        /// <summary>
        /// The add rating.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="rating">
        /// The rating.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool AddRating(TK id, Rating rating);

        /// <summary>
        /// The get all ratings.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TR"/>.
        /// </returns>
        TR GetAllRatings(TK id);
    }
}