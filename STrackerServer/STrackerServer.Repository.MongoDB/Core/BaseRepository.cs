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
    using System.Collections.Generic;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Exception;
    using STrackerServer.Logger.Core;

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
        protected readonly MongoDatabase Database;

        /// <summary>
        /// The STracker error logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T,TK}"/> class.
        /// </summary>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        /// <param name="logger">
        /// The STracker error logger.
        /// </param>
        protected BaseRepository(MongoClient client, MongoUrl url, ILogger logger)
        {
            this.Database = client.GetServer().GetDatabase(url.DatabaseName);
            this.logger = logger;
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
        public bool Create(T entity)
        {
            try
            {
                this.HookCreate(entity);
                return true;
            }
            catch (WriteConcernException exception)
            {
                this.logger.Error("Create", exception.GetType().Name, exception.Message);
                return false;
            }
            catch (MongoException exception)
            {
                this.logger.Error("Create", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
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
        public T Read(TK id)
        {
            try
            {
                return this.HookRead(id);
            }
            catch (MongoException exception)
            {
                this.logger.Error("Create", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Update(T entity)
        {
            try
            {
                // Increments the version number.
                entity.Version++;
                this.HookUpdate(entity);
                return true;
            }
            catch (WriteConcernException exception)
            {
                this.logger.Error("Update", exception.GetType().Name, exception.Message);
                return false;
            }
            catch (MongoException exception)
            {
                this.logger.Error("Update", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Delete(TK id)
        {
            try
            {
                this.HookDelete(id);
                return true;
            }
            catch (MongoException exception)
            {
                this.logger.Error("Delete", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
        }

        /// <summary>
        /// The read all.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<T> ReadAll()
        {
            try
            {
                return this.HookReadAll();
            }
            catch (MongoException exception)
            {
                this.logger.Error("ReadAll", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
        }

        /// <summary>
        /// The modify list.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="update">
        /// The update.
        /// </param>
        /// <param name="entity">
        /// The entity that have an modification in one of his lists.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool ModifyList(MongoCollection collection, IMongoQuery query, IMongoUpdate update, T entity)
        {
            try
            {
                collection.FindAndModify(query, SortBy.Null, update);
                
                // Update the entity version number.
                update = Update<T>.Set(e => e.Version, entity.Version + 1);
                collection.FindAndModify(query, SortBy.Null, update);
                
                return true;
            }
            catch (MongoException exception)
            {
                this.logger.Error("Modifylist", exception.GetType().Name, exception.Message);
                throw new STrackerDatabaseException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract void HookCreate(T entity);

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract T HookRead(TK id);

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract void HookUpdate(T entity);

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract void HookDelete(TK id);

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        protected abstract ICollection<T> HookReadAll();
    }
}