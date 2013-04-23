// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of base repository. This repository connects with MongoDB database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using MongoDB.Bson.Serialization;
using STrackerServer.DataAccessLayer.DomainEntities;

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;
    using System.Threading.Tasks;

    using global::MongoDB.Driver;

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
        /// Initializes static members of the <see cref="BaseRepository{T,TK}"/> class.
        /// </summary>
        static BaseRepository()
        {
            BsonClassMap.RegisterClassMap<Person>(
                cm =>
                {
                    cm.AutoMap();

                    // map _id field to key property.
                    cm.SetIdMember(cm.GetMemberMap(p => p.Key));
                });
            BsonClassMap.RegisterClassMap<Actor>();
            BsonClassMap.RegisterClassMap<User>();

            BsonClassMap.RegisterClassMap<Media>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);

                    // ignoring _id field when deserialize.
                    cm.SetIgnoreExtraElementsIsInherited(true);
                    cm.SetIgnoreExtraElements(true);
                });
            BsonClassMap.RegisterClassMap<TvShow>();

            BsonClassMap.RegisterClassMap<Season>(
               cm =>
               {
                   cm.AutoMap();
                   cm.UnmapProperty(c => c.Key);

                   // ignoring _id field when deserialize.
                   cm.SetIgnoreExtraElementsIsInherited(true);
                   cm.SetIgnoreExtraElements(true);
               });

            BsonClassMap.RegisterClassMap<Episode>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);

                    // ignoring _id field when deserialize.
                    cm.SetIgnoreExtraElementsIsInherited(true);
                    cm.SetIgnoreExtraElements(true);
                });
        } 

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
        /// Get one entity.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public abstract T Read(TK key);

        /// <summary>
        /// Create method. 
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        public abstract bool Create(T entity);

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        public abstract bool Update(T entity);

        /// <summary>
        /// Delete method.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Is abstract because his implementation is diferent from repository to repository.
        public abstract bool Delete(TK key);

        /// <summary>
        /// Try get the information from external provider.
        /// </summary>
        /// <param name="func">
        /// The function.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// After get the information from the external provider, inserts into our database.
        protected T TryGetFromProvider(Func<TK, T> func, TK key)
        {
            T entity;
            try
            {
                entity = func(key);

                var createResult = false;
                
                var createTask = Task.Factory.StartNew(() => { createResult = this.Create(entity); });

                createTask.ContinueWith(
                    completed =>
                        { 
                            completed.Wait();

                            if (!createResult)
                            {
                                // TODO, error occurred in create method. 
                            }
                        });
            }
            catch (Exception)
            {
                return default(T);
            }

            return entity;
        }
    }
}