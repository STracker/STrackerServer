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
    using System;
    using System.Diagnostics.CodeAnalysis;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;

    /// <summary>
    /// Base repository for MongoDB database.
    /// </summary>
    /// <typeparam name="T">
    /// Type of entity.
    /// </typeparam>
    /// <typeparam name="TK">
    /// Type of entity's Key.
    /// </typeparam>
    public abstract class BaseRepository<T, TK> : IRepository<T, TK> where T : IEntity<TK>
    {
        /// <summary>
        /// MongoDB database object.
        /// </summary>
        protected readonly MongoDatabase Database;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T,TK}"/> class.
        /// </summary>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        protected BaseRepository(MongoClient client, MongoUrl url)
        {
            this.Database = client.GetServer().GetDatabase(url.DatabaseName);
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
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return false;
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
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public T Read(TK id)
        {
            try
            {
                var entity = this.HookRead(id);
                if (Equals(entity, default(T)))
                {
                    return default(T);
                }

                entity.Key = id;
                return entity;
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return default(T);
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Update(T entity)
        {
            try
            {
                this.HookUpdate(entity);
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public void Delete(TK id)
        {
            try
            {
                this.HookDelete(id);
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
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
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool ModifyList(MongoCollection collection, IMongoQuery query, IMongoUpdate update)
        {
            try
            {
                return collection.FindAndModify(query, SortBy.Null, update).Ok;
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return false;
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
    }
}