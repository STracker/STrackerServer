// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base repository. This repository connects with MongoDB database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Configuration;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;

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
            // Get the URL for MongoClient. The URL it is defined in Application configuration file.
            var url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);

            // Create the database instance object.
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
        public bool Create(T entity)
        {
            var collection = this.GetDocumentCollection(entity.Id);
            return this.Create(entity, collection);
        }

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
            var collection = this.GetDocumentCollection(id);

            T entity;
            if ((entity = collection.FindOneByIdAs<T>(id.ToString())).Equals(default(T)))
            {
                // TODO, if the entity doesnt exists in MongoDB database, is necessary to call one provider.
                throw new System.NotImplementedException();
            }

            return entity;
        }

        /// <summary>
        /// Update one entity.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Update(T entity)
        {
            var collection = this.GetDocumentCollection(entity.Id);
            return this.Update(entity, collection);
        }

        /// <summary>
        /// Delete one entity.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Delete(TK id)
        {
            var collection = this.GetDocumentCollection(id);
            return this.Delete(id, collection);
        }

        /// <summary>
        /// Hook method for Create method. 
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract bool Create(T entity, MongoCollection collection);

        /// <summary>
        /// Hook method for Update method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract bool Update(T entity, MongoCollection collection);

        /// <summary>
        /// Hook method for Delete method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract bool Delete(TK id, MongoCollection collection);

        /// <summary>
        /// Hook method for getting collection of the document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="MongoCollection"/>.
        /// </returns>
        protected abstract MongoCollection GetDocumentCollection(TK id);
    }
}
