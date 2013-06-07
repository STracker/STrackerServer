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
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

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
            return this.collection.Update(
                Query<User>.EQ(user => user.Key, entity.Key),
                Update<User>.Set(user => user.Name, entity.Name).Set(user => user.Photo, entity.Photo)).Ok;
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
            var query = Query<User>.EQ(user => user.Key, key);
            return this.collection.FindAndRemove(query, SortBy.Null).Ok;
        }

        /// <summary>
        /// The add subscription.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSubscription(User user, TvShow tvshow)
        {
            user.SubscriptionList.Add(tvshow.GetSynopsis());
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            var update = Update<User>.Set(user1 => user1.SubscriptionList, user.SubscriptionList);
            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSubscription(User user, TvShow tvshow)
        {
            user.SubscriptionList.RemoveAt(user.SubscriptionList.FindIndex(synopsis => synopsis.Id.Equals(tvshow.Key)));
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            var update = Update<User>.Set(user1 => user1.SubscriptionList, user.SubscriptionList);
            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Invite(User from, User to)
        {
            var query = Query<User>.EQ(user => user.Key, to.Key);

            var update = Update<User>.AddToSet(user => user.FriendRequests, from.GetSynopsis());

            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The accept invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AcceptInvite(User from, User to)
        {
            var query = Query<User>.EQ(user => user.Key, to.Key);

            // adicionar amigo ( remover & adicionar)
            // remover request
            var update = Update<User>
                .AddToSet(user => user.Friends, from.GetSynopsis())
                .Pull(user => user.FriendRequests, from.GetSynopsis());

            if (!this.collection.Update(query, update).Ok)
            {
                return false;
            }

            query = Query<User>.EQ(user => user.Key, from.Key);

            update = Update<User>.AddToSet(user => user.Friends, to.GetSynopsis());

            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The reject invite.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RejectInvite(User from, User to)
        {
            var query = Query<User>.EQ(user => user.Key, to.Key);

            var update = Update<User>.Pull(user => user.FriendRequests, from.GetSynopsis());

            return this.collection.Update(query, update).Ok;
        }
    }
}
