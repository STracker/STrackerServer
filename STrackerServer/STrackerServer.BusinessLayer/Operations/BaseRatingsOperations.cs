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
    using System.Configuration;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

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
    public class BaseRatingsOperations<T, TR, TK> : BaseCrudOperations<IRatingsRepository<TR, TK>, TR, TK>, IRatingsOperations<TR, TK> where T : IEntity<TK> where TR : RatingsBase<TK>
    {
        /// <summary>
        /// The min rating.
        /// </summary>
        protected readonly int MinRating;

        /// <summary>
        /// The max rating.
        /// </summary>
        protected readonly int MaxRating;

        /// <summary>
        /// The entity repository.
        /// </summary>
        protected readonly IRepository<T, TK> EntityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRatingsOperations{T,TR,TK}"/> class.
        /// </summary>
        /// <param name="ratingsRepository">
        /// The ratings repository.
        /// </param>
        /// <param name="entityRepository">
        /// The entity Repository.
        /// </param>
        public BaseRatingsOperations(IRatingsRepository<TR, TK> ratingsRepository, IRepository<T, TK> entityRepository)
            : base(ratingsRepository)
        {
            this.EntityRepository = entityRepository;

            this.MinRating = int.Parse(ConfigurationManager.AppSettings["RatingMinValue"]);
            this.MaxRating = int.Parse(ConfigurationManager.AppSettings["RatingMaxValue"]);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public override TR Read(TK id)
        {
            return this.Repository.Read(id);
        }

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
        public bool AddRating(TK id, Rating rating)
        {
            if (rating.UserRating < this.MinRating || rating.UserRating > this.MaxRating)
            {
                return false;
            }

            var entity = this.EntityRepository.Read(id);
            return !Equals(entity, default(T)) && this.Repository.AddRating(id, rating);
        }
    }
}
