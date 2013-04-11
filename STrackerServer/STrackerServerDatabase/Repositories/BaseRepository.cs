// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base repository. This repository connects with MongoDB database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using System.Configuration;

    using MongoDB.Driver;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// Base repository for MongoDB database.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Id.
    /// </typeparam>
    public abstract class BaseRepository<T, TK> : IRepository<T, TK> where T : IEntity<TK>
    {
        /// <summary>
        /// MongoDB database object.
        /// </summary>
        private readonly MongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T,TK}"/> class.
        /// </summary>
        protected BaseRepository()
        {
            var url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);
            this.database = new MongoClient(url).GetServer().GetDatabase(url.DatabaseName);
        }

        /// <summary>
        /// Gets the MongoDB database object.
        /// </summary>
        protected MongoDatabase Database
        {
            get
            {
                return this.database;
            }
        }

        /// <summary>
        /// Create one entity. 
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Its abstract because his implementation is diferent from repository to repository.
        public abstract bool Create(T entity);

        /// <summary>
        /// Update one entity.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Its abstract because his implementation is diferent from repository to repository.
        public abstract bool Update(T entity);

        /// <summary>
        /// Delete one entity.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Its abstract because his implementation is diferent from repository to repository.
        public abstract bool Delete(TK id);

        /// <summary>
        /// Get one entity.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Read(TK id)
        {
            var collection = this.GetCollection(id.ToString());
            
            return collection.FindOneByIdAs<T>(id.ToString());
        }

        /// <summary>
        /// Get the collection where the document is.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="MongoCollection"/>.
        /// </returns>
        protected MongoCollection GetCollection(string id)
        {
            var tvshowId = id.Split(',')[0];

            return this.Database.GetCollection(tvshowId);
        }
    }
}
