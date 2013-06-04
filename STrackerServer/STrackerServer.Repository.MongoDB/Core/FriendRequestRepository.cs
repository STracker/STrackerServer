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
    public class FriendRequestRepository : IFriendRequestRepository
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
                            cm.UnmapProperty(c => c.Key);

                            cm.SetIgnoreExtraElementsIsInherited(true);
                            cm.SetIgnoreExtraElements(true);
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
        {
            this.collection = client.GetServer().GetDatabase(url.DatabaseName).GetCollection(CollectioneName);
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
            var query = Query<FriendRequest>.EQ(request => request.To, userId);
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
            var query = Query<FriendRequest>.EQ(request => request.From, userId);
            return this.collection.FindAs<FriendRequest>(query).ToList();
        }
    }
}
