// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IUsersRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.UsersRepositories
{
    using System.Collections.Generic;
    using System.Configuration;

    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The users repository.
    /// </summary>
    public class UsersRepository : BaseRepository<User, string>, IUsersRepository
    {
        /// <summary>
        /// The collection name.
        /// </summary>
        private readonly string collectioneName;

        /// <summary>
        /// The collection. In this case the collection is always the same (collection with name "Users").
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes static members of the <see cref="UsersRepository"/> class.
        /// </summary>
        static UsersRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(User)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<User>(
                cm =>
                {
                    cm.AutoMap();
                    cm.SetIdMember(cm.GetMemberMap(user => user.Key));
                });
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
            this.collectioneName = ConfigurationManager.AppSettings["UsersCollection"];
            this.collection = this.Database.GetCollection<User>(this.collectioneName);
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
        public bool AddSubscription(User user, TvShow.TvShowSynopsis tvshow)
        {
            var query = Query<User>.EQ(u => u.Key, user.Key);
            var update = Update<User>.AddToSet(u => u.SubscriptionList, tvshow);
            return this.ModifyList(this.collection, query, update);
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
        public bool RemoveSubscription(User user, TvShow.TvShowSynopsis tvshow)
        {
            var query = Query<User>.EQ(u => u.Key, user.Key);
            var update = Update<User>.Pull(u => u.SubscriptionList, tvshow);
            return this.ModifyList(this.collection, query, update);
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
            return this.ModifyList(this.collection, query, update);
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
            var update = Update<User>.AddToSet(user => user.Friends, from.GetSynopsis()).Pull(user => user.FriendRequests, from.GetSynopsis());
            if (!this.ModifyList(this.collection, query, update))
            {
                return false;
            }

            query = Query<User>.EQ(user => user.Key, from.Key);
            update = Update<User>.AddToSet(user => user.Friends, to.GetSynopsis());
            return this.ModifyList(this.collection, query, update);
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
            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The send suggestion.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(User userFrom, Suggestion suggestion)
        {
            var query = Query<User>.EQ(user => user.Key, userFrom.Key);
            var update = Update<User>.AddToSet(user => user.Suggestions, suggestion);

            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The remove suggestion.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSuggestion(User userFrom, Suggestion suggestion)
        {
            var query = Query<User>.EQ(user => user.Key, userFrom.Key);
            var update = Update<User>.Pull(user => user.Suggestions, suggestion);

            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The get suggestions.
        /// </summary>
        /// <param name="userFrom">
        /// The user from.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<Suggestion> GetSuggestions(User userFrom)
        {
            return this.Read(userFrom.Key).Suggestions;
        }

        /// <summary>
        /// Create one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(User entity)
        {
            this.collection.Insert(entity);
        }

        /// <summary>
        /// Get one user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        protected override User HookRead(string id)
        {
            return this.collection.FindOneByIdAs<User>(id);
        }

        /// <summary>
        /// Update one user.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(User entity)
        {
            var query = Query<User>.EQ(user => user.Key, entity.Key);
            var update = Update<User>.Set(user => user.Name, entity.Name).Set(user => user.Photo, entity.Photo);
            this.collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Delete one user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            var query = Query<User>.EQ(user => user.Key, id);
            this.collection.FindAndRemove(query, SortBy.Null);
        }
    }
}