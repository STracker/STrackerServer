// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using MongoDB.Driver;

    using STrackerServerDatabase.Core;

    /// <summary>
    /// The base repository. Contains the MongoDB collection that have the entities associated to repository.
    /// </summary>
    /// <typeparam name="TK">
    /// Type of id of entity.
    /// </typeparam>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    public abstract class Repository<TK, T> : IRepository<TK, T> where T : IEntity<TK>
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly MongoDatabase database;

        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TK,T}"/> class.
        /// </summary>
        /// <param name="collectionName">
        /// The collection name.
        /// </param>
        protected Repository(string collectionName)
        {
            this.database = DatabaseLocator.GetMongoDbDatabaseInstance();
            this.collection = this.database.GetCollection(collectionName);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        protected MongoCollection Collection
        { 
            get
            {
                return this.collection;
            }
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Create(T entity);

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public abstract T Read(TK id);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Update(T entity);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract bool Delete(TK id);
    }
}