// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRatingsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base ratings repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;
    using System.Configuration;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The base ratings repository.
    /// </summary>
    /// <typeparam name="TR">
    /// Type of the rating.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of the rating key.
    /// </typeparam>
    public abstract class BaseRatingsRepository<TR, TK> : BaseRepository<TR, TK>, IRatingsRepository<TR, TK>
        where TR : RatingsBase<TK>
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        protected readonly string CollectionPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRatingsRepository{TR,TK}"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        protected BaseRatingsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.CollectionPrefix = ConfigurationManager.AppSettings["RatingsCollection"];
        }

        /// <summary>
        /// The remove all ratings.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveAllRatings(TK id)
        {
            try
            {
                var collection = this.Database.GetCollection(string.Format("{0}-{1}", this.CollectionPrefix, id));
                collection.Drop();
                return true;
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
                return false;
            }
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
        public abstract bool AddRating(TK id, Rating rating);

        /// <summary>
        /// The remove rating.
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
        public abstract bool RemoveRating(TK id, Rating rating);
    }
}