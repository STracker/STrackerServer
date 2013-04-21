// --------------------------------------------------------------------------------------------------------------------
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
    using System;

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
        /// The collection. In this case the collection is always the same (collection with name "Users").
        /// </summary>
        private readonly MongoCollection collection;

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
            this.collection = this.Database.GetCollection<User>("Users");
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
        public override bool Create(User entity)
        {
            // TODO, make a index for email field
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
        public override User Read(string key)
        {
            var query = Query<User>.EQ(u => u.Email, key);

            return this.collection.FindOneAs<User>(query);
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
        public override bool Update(User entity)
        {
            throw new NotImplementedException();
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
        public override bool Delete(string key)
        {
            throw new NotImplementedException();
        }
    }
}
