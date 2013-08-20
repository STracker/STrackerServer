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
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Logger.Core;

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
        /// The collection of all genres synopsis.
        /// </summary>
        private readonly MongoCollection collectionOfSynopsis;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public UsersRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
            this.collection = this.Database.GetCollection<User>(ConfigurationManager.AppSettings["UsersCollection"]);
            this.collectionOfSynopsis = this.Database.GetCollection<User>(ConfigurationManager.AppSettings["UsersSynopsisCollection"]);
        }

        /// <summary>
        /// Get all users that have the same name of the name passed in parameters.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<User.UserSynopsis> ReadByName(string name)
        {
            var query = Query<User>.Where(user => user.Name.ToLower().Contains(name.ToLower()));
            return this.collectionOfSynopsis.FindAs<User.UserSynopsis>(query).ToList();
        }

        /// <summary>
        /// Add one subscription to user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSubscription(string id, Subscription subscription)
        {
            var query = Query<User>.EQ(u => u.Id, id);
            var update = Update<User>.AddToSet(u => u.Subscriptions, subscription);
            return this.ModifyList(this.collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Remove one subscription from user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSubscription(string id, Subscription subscription)
        {
            var query = Query<User>.EQ(u => u.Id, id);
            var update = Update<User>.Pull(u => u.Subscriptions, subscription);
            return this.ModifyList(this.collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Invite one user to make part of the friends list.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InviteFriend(string userFromId, string userToId)
        {
            var query = Query<User>.EQ(user => user.Id, userToId);
            var update = Update<User>.AddToSet(user => user.FriendRequests, this.Read(userFromId).GetSynopsis());
            return this.ModifyList(this.collection, query, update, this.Read(userToId));
        }

        /// <summary>
        /// Accept invitation from one user to be part of the friends list.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AcceptInvite(string userFromId, string userToId)
        {
            var userFrom = this.Read(userFromId);
            var userTo = this.Read(userToId);

            var query = Query<User>.EQ(user => user.Id, userToId);
            var update = Update<User>.Pull(user => user.FriendRequests, userFrom.GetSynopsis()).AddToSet(user => user.Friends, userFrom.GetSynopsis());
            if (!this.ModifyList(this.collection, query, update, userTo))
            {
                return false;
            }

            query = Query<User>.EQ(user => user.Id, userFromId);
            update = Update<User>.AddToSet(user => user.Friends, userTo.GetSynopsis());
            return this.ModifyList(this.collection, query, update, userFrom);
        }

        /// <summary>
        /// Reject invitation from one user.
        /// </summary>
        /// <param name="userFromId">
        /// The user, that invites, id.
        /// </param>
        /// <param name="userToId">
        /// The user, that receive the invite, id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RejectInvite(string userFromId, string userToId)
        {
            var query = Query<User>.EQ(user => user.Id, userToId);
            var update = Update<User>.Pull(user => user.FriendRequests, this.Read(userFromId).GetSynopsis());
            return this.ModifyList(this.collection, query, update, this.Read(userToId));
        }

        /// <summary>
        /// Remove one friend from user's friends list.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="friendId">
        /// The friend id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveFriend(string id, string friendId)
        {
            var user = this.Read(id);
            var friend = this.Read(friendId);

            return this.RemoveFriendFromUser(user, friend) && this.RemoveFriendFromUser(friend, user);
        }

        /// <summary>
        /// Send one television show suggestion to one user.
        /// </summary>
        /// <param name="userToId">
        /// The user that receives the suggestion.
        /// </param>
        /// <param name="suggestion">
        /// The suggestion.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(string userToId, Suggestion suggestion)
        {
            var query = Query<User>.EQ(user => user.Id, userToId);
            var update = Update<User>.AddToSet(user => user.Suggestions, suggestion);
            return this.ModifyList(this.collection, query, update, this.Read(userToId));
        }

        /// <summary>
        /// Remove all suggestions of one television show.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveTvShowSuggestions(string id, string tvshowId)
        {
            var user = this.Read(id);
            var query = Query<User>.EQ(user1 => user1.Id, id);
            var update = Update<User>.PullAll(u => u.Suggestions, user.Suggestions.Where(suggestion => suggestion.TvShow.Id.Equals(tvshowId)));
            return this.ModifyList(this.collection, query, update, user);
        }

        /// <summary>
        /// Add one new watched episode to episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episode">
        /// The episode watched synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddWatchedEpisode(string id, Episode.EpisodeSynopsis episode)
        {
            var user = this.Read(id);
            user.Subscriptions.Find(subscription => subscription.TvShow.Id.Equals(episode.Id.TvShowId)).EpisodesWatched.Add(episode);
            return this.Update(user);
        }

        /// <summary>
        /// Remove one new watched episode from episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episode">
        /// The episode watched synopsis.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveWatchedEpisode(string id, Episode.EpisodeSynopsis episode)
        {
            var user = this.Read(id);
            var sub = user.Subscriptions.Find(subscription => subscription.TvShow.Id.Equals(episode.Id.TvShowId));
            return sub.EpisodesWatched.Remove(episode) && this.Update(user);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(User entity)
        {
            // Ensure index, for search by name.
            this.collectionOfSynopsis.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // Create document in synpsis collection.
            this.collectionOfSynopsis.Insert(entity.GetSynopsis());
            
            this.collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
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
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(User entity)
        {
            var query = Query<User>.EQ(u => u.Id, entity.Id);
            
            // Update the synopsis in the synopsis collection.
            var update = Update<User.UserSynopsis>.Replace(entity.GetSynopsis());
            this.collectionOfSynopsis.FindAndModify(query, SortBy.Null, update);

            // Update the user document.
            update = Update<User>.Replace(entity);
            this.collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<User> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
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
            var update = Update<User>.Pull(userInput => userInput.Friends, friend.GetSynopsis()).PullAll(userInput => userInput.Suggestions, user.Suggestions.Where(suggestion => suggestion.User.Id.Equals(friend.Id)));

            return this.ModifyList(this.collection, query, update, user);
        }
    }
}