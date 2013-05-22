// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FriendRequestRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IFriendRequestRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The friend request repository.
    /// </summary>
    public class FriendRequestRepository : BaseRepository<FriendRequest, string>, IFriendRequestRepository
    {
        /// <summary>
        /// The collection name.
        /// </summary>
        private const string CollectioneName = "FriendRequests";

        /// <summary>
        /// The collection.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes static members of the <see cref="FriendRequestRepository"/> class.
        /// </summary>
        static FriendRequestRepository()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(FriendRequest)))
            {
                BsonClassMap.RegisterClassMap<FriendRequest>(
                    cm =>
                        {
                            cm.AutoMap();

                            // map _id field to key property.
                            cm.SetIdMember(cm.GetMemberMap(p => p.Key));
                        });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendRequestRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public FriendRequestRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
            this.collection = this.Database.GetCollection<User>(CollectioneName);
        }

        /// <summary>
        /// The hook create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(FriendRequest entity)
        {
            return this.collection.Insert(entity).Ok;
        }

        /// <summary>
        /// The hook read.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="FriendRequest"/>.
        /// </returns>
        public override FriendRequest HookRead(string key)
        {
            return this.collection.FindOneByIdAs<FriendRequest>(key);
        }

        /// <summary>
        /// The hook update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(FriendRequest entity)
        {
            return this.collection.Save(entity).Ok;
        }

        /// <summary>
        /// The hook delete.
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

        /// <summary>
        /// The read all to.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<FriendRequest> ReadAllTo(string userId)
        {
            var query = Query.And(
                Query<FriendRequest>.EQ(request => request.To, userId),
                Query<FriendRequest>.EQ(request => request.Accepted, false));

            return this.collection.FindAs<FriendRequest>(query).ToList();
        }

        /// <summary>
        /// The read all from.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<FriendRequest> ReadAllFrom(string userId)
        {
            var query = Query.And(
               Query<FriendRequest>.EQ(request => request.From, userId),
               Query<FriendRequest>.EQ(request => request.Accepted, true));

            return this.collection.FindAs<FriendRequest>(query).ToList();
        }
    }
}
