// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.UsersOperations
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using STrackerServer.BusinessLayer.Calendar;
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Permissions;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The users operations.
    /// </summary>
    public class UsersOperations : BaseCrudOperations<IUsersRepository, User, string>, IUsersOperations
    {
        /// <summary>
        /// The television shows operations.
        /// </summary>
        private readonly ITvShowsOperations tvshowsRepository;

        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// The episodes operations.
        /// </summary>
        private readonly IEpisodesOperations episodesOperations;

        /// <summary>
        /// The new episodes operations.
        /// </summary>
        private readonly ITvShowNewEpisodesOperations newEpisodesOperations;

        /// <summary>
        /// The permission manager.
        /// </summary>
        private readonly IPermissionManager<Permissions, int> permissionManager;

        /// <summary>
        /// The calendar builder.
        /// </summary>
        private readonly ICalendar calendarBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="tvshowsRepository">
        /// The television shows repository.
        /// </param>
        /// <param name="seasonsOperations">
        /// The seasons operations.
        /// </param>
        /// <param name="episodesOperations">
        /// The episodes operations.
        /// </param>
        /// <param name="newEpisodesOperations">
        /// The new Episodes operations.
        /// </param>
        /// <param name="permissionManager">
        /// The permission manager.
        /// </param>
        /// <param name="calendarBuilder">
        /// The calendar builder
        /// </param>
        public UsersOperations(
            IUsersRepository repository,
            ITvShowsOperations tvshowsRepository,
            ISeasonsOperations seasonsOperations,
            IEpisodesOperations episodesOperations,
            ITvShowNewEpisodesOperations newEpisodesOperations,
            IPermissionManager<Permissions, int> permissionManager,
            ICalendar calendarBuilder)
            : base(repository)
        {
            this.tvshowsRepository = tvshowsRepository;
            this.seasonsOperations = seasonsOperations;
            this.episodesOperations = episodesOperations;
            this.newEpisodesOperations = newEpisodesOperations;
            this.permissionManager = permissionManager;
            this.calendarBuilder = calendarBuilder;
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="User"/>.
        /// </returns>
        public override User Read(string id)
        {
            return this.Repository.Read(id);
        }

        /// <summary>
        /// Verify if the user exists, if not create one. Also verify if the properties of the user have changed.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void VerifyAndSave(User user)
        {
            var domainUser = this.Repository.Read(user.Id);
            if (domainUser == null)
            {
                this.Create(user);
                return;
            }

            domainUser.Name = user.Name;
            domainUser.Email = user.Email;

            this.Update(domainUser);
        }

        /// <summary>
        /// Get all users that have the same name of the name passed in parameters.
        /// </summary>
        /// <param name="name">
        /// The name for search.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<User.UserSynopsis> ReadByName(string name, Range range = null)
        {
            if (name == null || string.Empty.Equals(name))
            {
                return new List<User.UserSynopsis>();
            }

            if (range == null)
            {
                range = new Range { Start = 0, End = int.MaxValue };
            }
            else
            {
                if (range.Start > range.End)
                {
                    return new List<User.UserSynopsis>();
                }
            }

            return this.Repository.ReadByName(name, range);
        }

        /// <summary>
        /// Add one subscription to user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSubscription(string id, string tvshowId)
        {
            var user = this.Read(id);
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (user == null || tvshow == null)
            {
                return false;
            }

            if (user.Subscriptions.Exists(sub => sub.TvShow.Id.Equals(tvshowId)))
            {
                return false;
            }

            this.Repository.RemoveTvShowSuggestions(id, tvshowId);

            return this.Repository.AddSubscription(id, new Subscription { TvShow = tvshow.GetSynopsis() });
        }

        /// <summary>
        /// Remove one subscription from user's subscription list.
        /// </summary>
        /// <param name="id">
        /// The id of the user.
        /// </param>
        /// <param name="tvshowId">
        /// The television show Id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSubscription(string id, string tvshowId)
        {
            var user = this.Read(id);
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (user == null || tvshow == null)
            {
                return false;
            }

            var subscription = user.Subscriptions.Find(sub => sub.TvShow.Id.Equals(tvshowId));

            if (subscription == null)
            {
                return false;
            }

            return this.Repository.RemoveSubscription(id, subscription);
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
            if (userFromId.Equals(userToId))
            {
                return false;
            }

            var userTo = this.Read(userToId);

            if (this.Read(userFromId) == null || userTo == null)
            {
                return false;
            }

            if (userTo.FriendRequests.Exists(fr => fr.Id.Equals(userFromId)) || userTo.Friends.Exists(friend => friend.Id.Equals(userFromId)))
            {
                return false;
            }

            return this.Repository.InviteFriend(userFromId, userToId);
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
            User user;
            if (this.Read(userFromId) == null || (user = this.Read(userToId)) == null)
            {
                return false;
            }

            if (!user.FriendRequests.Exists(fr => fr.Id.Equals(userFromId)))
            {
                return false;
            }

            return this.Repository.AcceptInvite(userFromId, userToId);
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
            User user;
            if (this.Read(userFromId) == null || (user = this.Read(userToId)) == null)
            {
                return false;
            }

            if (!user.FriendRequests.Exists(fr => fr.Id.Equals(userFromId)))
            {
                return true;
            }

            return this.Repository.RejectInvite(userFromId, userToId);
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
            User user;
            if ((user = this.Read(id)) == null || this.Read(friendId) == null)
            {
                return false;
            }

            if (!user.Friends.Exists(fr => fr.Id.Equals(friendId)))
            {
                return false;
            }

            return this.Repository.RemoveFriend(id, friendId);
        }

        /// <summary>
        /// Send one television show suggestion to one user.
        /// </summary>
        /// <param name="userFrom">
        /// The user that sends the suggestion.
        /// </param>
        /// <param name="userTo">
        /// The user that receives the suggestion.
        /// </param>
        /// <param name="tvshowId">
        /// The television show id.
        ///  </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendSuggestion(string userFrom, string userTo, string tvshowId)
        {
            var user = this.Read(userFrom);
            var friend = this.Read(userTo);
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            if (user == null || friend == null || tvshow == null)
            {
                return false;
            }

            return this.Repository.SendSuggestion(userTo, new Suggestion { TvShow = tvshow.GetSynopsis(), User = user.GetSynopsis() });
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
            return this.Read(id) != null && this.Repository.RemoveTvShowSuggestions(id, tvshowId);
        }

        /// <summary>
        /// Add one new watched episode to episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episodeId">
        /// The episode id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddWatchedEpisode(string id, Episode.EpisodeId episodeId)
        {
            var user = this.Repository.Read(id);
            var episode = this.episodesOperations.Read(episodeId);

            if (user == null || episode == null)
            {
                return false;
            }

            var episodeDate = DateTime.ParseExact(episode.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var systemDate = DateTime.UtcNow;
            if (DateTime.Compare(episodeDate, systemDate) > 0)
            {
                return false;
            }

            var subscription = user.Subscriptions.Find(sub => sub.TvShow.Id.Equals(episode.Id.TvShowId));
            if (subscription == null)
            {
                return false;
            }

            if (subscription.EpisodesWatched.Exists(epis => epis.Equals(episode.GetSynopsis())))
            {
                return false;
            }

            return this.Repository.AddWatchedEpisode(id, episode.GetSynopsis());
        }

        /// <summary>
        /// Remove one new watched episode from episodes watched list in the television show 
        /// subscription.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="episodeId">
        /// The episode id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveWatchedEpisode(string id, Episode.EpisodeId episodeId)
        {
            var user = this.Repository.Read(id);
            var episode = this.episodesOperations.Read(episodeId);

            if (user == null || episode == null)
            {
                return false;
            }

            var subscription = user.Subscriptions.Find(sub => sub.TvShow.Id.Equals(episode.Id.TvShowId));
            if (subscription == null)
            {
                return false;
            }

            if (!subscription.EpisodesWatched.Exists(epis => epis.Equals(episode.GetSynopsis())))
            {
                return false;
            }

            return this.Repository.RemoveWatchedEpisode(id, episode.GetSynopsis());
        }

        /// <summary>
        /// Get the new episodes from user's subscription list.
        /// If the date is null, return all new episodes from all television shows in subscription list.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<TvShowCalendar> GetUserNewEpisodes(string userId, DateTime? date)
        {
            User user;
            if ((user = this.Read(userId)) == null)
            {
                return null;
            }

            var calendar = new List<TvShowCalendar>();
            foreach (var subscription in user.Subscriptions)
            {
                var episodes = this.newEpisodesOperations.GetNewEpisodes(subscription.TvShow.Id, date);
                foreach (var episode in episodes.Episodes)
                {
                    var day = calendar.FirstOrDefault(c => c.Date.Equals(episode.Date));
                    if (day != null)
                    {
                        var exists = false;
                        foreach (var entry in day.Entries)
                        {
                            if (entry.TvShow.Id.Equals(episode.Id.TvShowId))
                            {
                                entry.Episodes.Add(episode);
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            var entry = new TvShowCalendar.TvShowCalendarTvShowEntry { TvShow = subscription.TvShow };
                            entry.Episodes.Add(episode);
                            day.Entries.Add(entry);
                        }
                    }
                    else
                    {
                        var newDay = new TvShowCalendar { Date = episode.Date };
                        var entry = new TvShowCalendar.TvShowCalendarTvShowEntry { TvShow = subscription.TvShow };
                        entry.Episodes.Add(episode);
                        newDay.Entries.Add(entry);
                        calendar.Add(newDay);
                    }
                }
            }

            calendar = calendar.OrderBy(tvshowCalendar => tvshowCalendar.Date).ToList();

            return calendar;
        }

        /// <summary>
        /// Change user permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="permission">
        /// The permission value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SetUserPermission(string userId, int permission)
        {
            var user = this.Read(userId);

            if (user == null)
            {
                return false;
            }

            var permissions = this.permissionManager.GetPermissions();

            if (!permissions.ContainsKey((Permissions)permission))
            {
                return false;
            }

            user.Permission = permission;
            return this.Update(user);
        }

        /// <summary>
        /// Get the user's new episodes calendar.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>byte[]</cref>
        ///     </see> .
        /// </returns>
        public byte[] GetCalendar(string userId)
        {
            return this.Read(userId) == null ? null : this.calendarBuilder.Create(this.GetUserNewEpisodes(userId, null));
        }

        /// <summary>
        /// Mark all episodes from one season has watched.
        /// </summary>
        /// <param name="id">
        /// The user id.
        /// </param>
        /// <param name="seasonId">
        /// The season id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSeasonWatched(string id, Season.SeasonId seasonId)
        {
            var season = this.seasonsOperations.Read(seasonId);
            foreach (var episode in season.Episodes)
            {
                this.AddWatchedEpisode(id, episode.Id);
            }

            return true;
        }
    }
}