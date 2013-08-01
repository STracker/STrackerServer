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
    using System.Linq;

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
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSubscription(User user, Subscription subscription)
        {
            var query = Query<User>.EQ(u => u.Key, user.Key);
            var update = Update<User>.AddToSet(u => u.SubscriptionList, subscription);
            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The remove subscription.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSubscription(User user, Subscription subscription)
        {
            var query = Query<User>.EQ(u => u.Key, user.Key);
            var update = Update<User>.Pull(u => u.SubscriptionList, subscription);
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
            var update = Update<User>.Pull(user => user.FriendRequests, from.GetSynopsis()).AddToSet(user => user.Friends, from.GetSynopsis());
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
        /// <param name="userTo">
        /// The user To.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(User userTo, Suggestion suggestion)
        {
            var query = Query<User>.EQ(user => user.Key, userTo.Key);
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
        /// The find by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<User> FindByName(string name)
        {
            var query = Query<User>.Where(user => user.Name.ToLower().Contains(name.ToLower()));
            return this.collection.FindAs<User>(query).ToList();
        }

        /// <summary>
        /// The remove friend.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <param name="userFriend">
        /// The user friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveFriend(User userModel, User userFriend)
        {
            return this.RemoveFriendInfoFromUser(userModel, userFriend) && this.RemoveUserInfoFromFriend(userModel, userFriend);
        }

        /// <summary>
        /// The remove television show suggestions.
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
        public bool RemoveTvShowSuggestions(User user, TvShow tvshow)
        {
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            var update = Update<User>.PullAll(user1 => user1.Suggestions, user.Suggestions.Where(suggestion => suggestion.TvShow.Id.Equals(tvshow.Key)));

            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The add watched episode.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddWatchedEpisode(User user, Episode.EpisodeSynopsis episode)
        {
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            user.SubscriptionList.Find(subscription => subscription.TvShow.Id.Equals(episode.TvShowId)).EpisodesWatched.Add(episode);
            var update = Update<User>.Set(user1 => user1.SubscriptionList, user.SubscriptionList);
            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The remove watched episode.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveWatchedEpisode(User user, Episode.EpisodeSynopsis episode)
        {
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            user.SubscriptionList.Find(subscription => subscription.TvShow.Id.Equals(episode.TvShowId)).EpisodesWatched.Remove(episode);
            var update = Update<User>.Set(user2 => user2.SubscriptionList, user.SubscriptionList);
            return this.collection.Update(query, update).Ok;
        }

        /// <summary>
        /// The set user permission.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SetUserPermission(User user, int permission)
        {
            var query = Query<User>.EQ(user1 => user1.Key, user.Key);
            var update = Update<User>.Set(user2 => user2.Permission, permission);
            return this.collection.Update(query, update).Ok;
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

        /// <summary>
        /// The remove friend info from user.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <param name="userFriend">
        /// The user friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool RemoveFriendInfoFromUser(User userModel, User userFriend)
        {
            var query = Query<User>.EQ(user => user.Key, userModel.Key);
            var update = Update<User>
                .Pull(user => user.Friends, userFriend.GetSynopsis())
                .PullAll(user => user.Suggestions, userModel.Suggestions.Where(suggestion => suggestion.User.Id.Equals(userFriend.Key)));

            return this.ModifyList(this.collection, query, update);
        }

        /// <summary>
        /// The remove user info from friend.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <param name="userFriend">
        /// The user friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool RemoveUserInfoFromFriend(User userModel, User userFriend)
        {
            var query = Query<User>.EQ(user => user.Key, userFriend.Key);
            var update = Update<User>
                .Pull(user => user.Friends, userModel.GetSynopsis())
                .PullAll(user => user.Suggestions, userFriend.Suggestions.Where(suggestion => suggestion.User.Id.Equals(userModel.Key)));

            return this.ModifyList(this.collection, query, update);
        }
    }
}