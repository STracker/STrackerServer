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
            this.collection = this.Database.GetCollection<User>(ConfigurationManager.AppSettings["UsersCollection"]);
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
            var query = Query<User>.EQ(u => u.Id, user.Id);
            var update = Update<User>.AddToSet(u => u.SubscriptionList, subscription);
            return this.ModifyList(this.collection, query, update, user);
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
            var query = Query<User>.EQ(u => u.Id, user.Id);
            var update = Update<User>.Pull(u => u.SubscriptionList, subscription);
            return this.ModifyList(this.collection, query, update, user);
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
            var query = Query<User>.EQ(user => user.Id, to.Id);
            var update = Update<User>.AddToSet(user => user.FriendRequests, from.GetSynopsis());
            return this.ModifyList(this.collection, query, update, to);
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
            var query = Query<User>.EQ(user => user.Id, to.Id);
            var update = Update<User>.Pull(user => user.FriendRequests, from.GetSynopsis()).AddToSet(user => user.Friends, from.GetSynopsis());
            if (!this.ModifyList(this.collection, query, update, to))
            {
                return false;
            }

            query = Query<User>.EQ(user => user.Id, from.Id);
            update = Update<User>.AddToSet(user => user.Friends, to.GetSynopsis());
            return this.ModifyList(this.collection, query, update, from);
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
            var query = Query<User>.EQ(user => user.Id, to.Id);
            var update = Update<User>.Pull(user => user.FriendRequests, from.GetSynopsis());
            return this.ModifyList(this.collection, query, update, to);
        }

        /// <summary>
        /// The send suggestion.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(User user, Suggestion suggestion)
        {
            var query = Query<User>.EQ(userInput => userInput.Id, user.Id);
            var update = Update<User>.AddToSet(userInput => userInput.Suggestions, suggestion);
            return this.ModifyList(this.collection, query, update, user);
        }

        /// <summary>
        /// The remove television show suggestions.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveTvShowSuggestions(User user, string tvshowId)
        {
            var query = Query<User>.EQ(user1 => user1.Id, user.Id);
            var update = Update<User>.PullAll(user1 => user1.Suggestions, user.Suggestions.Where(suggestion => suggestion.TvShow.Id.Equals(tvshowId)));
            return this.ModifyList(this.collection, query, update, user);
        }

        /// <summary>
        /// The get suggestions.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Suggestion> GetSuggestions(User user)
        {
            return this.Read(user.Id).Suggestions;
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
        public ICollection<User.UserSynopsis> FindByName(string name)
        {
            var query = Query<User>.Where(user => user.Name.ToLower().Contains(name.ToLower()));
            return this.collection.FindAs<User>(query).Select(user => user.GetSynopsis()).ToList();
        }

        /// <summary>
        /// The remove friend.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="friend">
        /// The friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveFriend(User user, User friend)
        {
            return this.RemoveFriendFromUser(user, friend) && this.RemoveFriendFromUser(friend, user);
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
            var query = Query<User>.EQ(user1 => user1.Id, user.Id);
            user.SubscriptionList.Find(subscription => subscription.TvShow.Id.Equals(episode.Id.TvshowId)).EpisodesWatched.Add(episode);
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
            var query = Query<User>.EQ(user1 => user1.Id, user.Id);
            user.SubscriptionList.Find(subscription => subscription.TvShow.Id.Equals(episode.Id.TvshowId)).EpisodesWatched.Remove(episode);
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
            var query = Query<User>.EQ(user1 => user1.Id, user.Id);
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
            var query = Query<User>.EQ(user => user.Id, entity.Id);
            var update = Update<User>.Set(user => user.Name, entity.Name).Set(user => user.Photo, entity.Photo).Set(user => user.Version, entity.Version + 1);
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
            var query = Query<User>.EQ(user => user.Id, id);
            this.collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// The remove friend info from user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="friend">
        /// The friend.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool RemoveFriendFromUser(User user, User friend)
        {
            var query = Query<User>.EQ(userInput => userInput.Id, user.Id);
            var update = Update<User>
                .Pull(userInput => userInput.Friends, friend.GetSynopsis())
                .PullAll(userInput => userInput.Suggestions, user.Suggestions.Where(suggestion => suggestion.User.Id.Equals(friend.Id)));

            return this.ModifyList(this.collection, query, update, user);
        }
    }
}