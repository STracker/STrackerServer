// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IRatingsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The ratings operations.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the entity.
    /// </typeparam>
    /// <typeparam name="TR">
    /// Type of the rating entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the key.
    /// </typeparam>
    public class BaseRatingsOperations<T, TR, TK> : IRatingsOperations<TR, TK>
        where T : IEntity<TK>
        where TR : BaseRatings<TK>
    {
        /// <summary>
        /// The min rating.
        /// </summary>
        protected readonly int MinRating = 1;

        /// <summary>
        /// The max rating.
        /// </summary>
        protected readonly int MaxRating = 5;

        /// <summary>
        /// The ratings repository.
        /// </summary>
        protected readonly IRatingsRepository<TR, TK> RatingsRepository;

        /// <summary>
        /// The entity repository.
        /// </summary>
        protected readonly IRepository<T, TK> Repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRatingsOperations{T,TR,TK}"/> class.
        /// </summary>
        /// <param name="ratingsRepository">
        /// The ratings repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public BaseRatingsOperations(IRatingsRepository<TR, TK> ratingsRepository, IRepository<T, TK> repository)
        {
            this.RatingsRepository = ratingsRepository;
            this.Repository = repository;
        } 

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
        public bool AddRating(TK key, Rating rating)
        {
            if (rating.UserRating < this.MinRating || rating.UserRating > this.MaxRating)
            {
                return false;
            }

            var entity = this.Repository.Read(key);

            return !Equals(entity, default(T)) && this.RatingsRepository.AddRating(key, rating);
        }

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
        public bool RemoveRating(TK key, Rating rating)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The get all ratings.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TR"/>.
        /// </returns>
        public TR GetAllRatings(TK key)
        {
            var entity = this.Repository.Read(key);

            return Equals(entity, default(T)) ? default(TR) : this.RatingsRepository.Read(key);
        }
    }
}
