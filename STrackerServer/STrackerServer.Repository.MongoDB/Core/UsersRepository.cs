﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IUsersRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The users repository.
    /// </summary>
    public class UsersRepository : BaseRepository<User, string>, IUsersRepository
    {
        /// <summary>
        /// The collection name.
        /// </summary>
        private const string CollectioneName = "Users";

        /// <summary>
        /// The collection. In this case the collection is always the same (collection with name "Users").
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes static members of the <see cref="UsersRepository"/> class.
        /// </summary>
        static UsersRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Person)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<Person>(
                cm =>
                    {
                        cm.AutoMap();

                        // map _id field to key property.
                        cm.SetIdMember(cm.GetMemberMap(p => p.Key));
                    });
            BsonClassMap.RegisterClassMap<Actor>();
            BsonClassMap.RegisterClassMap<User>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public UsersRepository(MongoClient client, MongoUrl url) : base(client, url)
        {
            this.collection = this.Database.GetCollection<User>(CollectioneName);
        }

        /// <summary>
        /// Create one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(User entity)
        {
            return this.collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Get one user.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public override User HookRead(string key)
        {
            return this.collection.FindOneByIdAs<User>(key);
        }

        /// <summary>
        /// Update one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(User entity)
        {
            return this.collection.Save(entity).Ok;
        }

        /// <summary>
        /// Delete one user.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(string key)
        {
            var query = Query<User>.EQ(u => u.Key, key);

            return this.collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
