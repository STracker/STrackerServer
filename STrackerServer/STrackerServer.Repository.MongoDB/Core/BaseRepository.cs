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
    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

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
            /*
             * Affects some serialization options of MongoDB class mapping like id field. 
             */

            BsonClassMap.RegisterClassMap<Media>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.SetIdMember(cm.GetMemberMap(c => c.Key));
                    });

            BsonClassMap.RegisterClassMap<TvShow>();

            BsonClassMap.RegisterClassMap<Person>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.SetIdMember(cm.GetMemberMap(c => c.Key));
                    });

            BsonClassMap.RegisterClassMap<Actor>();
        } 

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T,TK}"/> class.
        /// </summary>
        protected BaseRepository()
        {
            /*
             * The MongoDatabase class is Thread-Safe, only needs one instance of this class per database
             * if the settings to acess to it is always the sames. 
             */
            this.Database = MongoUtils.Database;
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
        public T Read(TK key)
        {
            var entity = this.HookRead(key);

            if (!entity.Equals(default(T)))
            {
                return entity;
            }

            // TODO, if the entity doesnt exists in MongoDB database, is necessary to call one provider.
            throw new System.NotImplementedException();
        }

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
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        protected abstract T HookRead(TK key);
    }
}