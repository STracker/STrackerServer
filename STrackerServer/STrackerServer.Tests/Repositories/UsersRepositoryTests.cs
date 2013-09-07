// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The users repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The users repository tests.
    /// </summary>
    [TestFixture]
    public class UsersRepositoryTests
    {
        /// <summary>
        /// The users repository.
        /// </summary>
        private readonly IUsersRepository usersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepositoryTests"/> class.
        /// </summary>
        public UsersRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.usersRepository = kernel.Get<IUsersRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            Assert.True(this.usersRepository.Create(Utils.CreateUser(Utils.CreateId())));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.usersRepository.Create(Utils.CreateUser(id)));
            Assert.False(this.usersRepository.Create(Utils.CreateUser(id)));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var id = Utils.CreateId();

            Assert.True(this.usersRepository.Create(Utils.CreateUser(id)));
            Assert.NotNull(this.usersRepository.Read(id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.usersRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.usersRepository.ReadAll());
        }

        /// <summary>
        /// Test read by name.
        /// </summary>
        [Test]
        public void ReadByName()
        {
            Assert.True(this.usersRepository.Create(Utils.CreateUser(Utils.CreateId())));
            Assert.True(this.usersRepository.Create(Utils.CreateUser(Utils.CreateId())));

            Assert.True(this.usersRepository.ReadByName("Name").Count >= 2);
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            var id = Utils.CreateId();

            Assert.True(this.usersRepository.Create(Utils.CreateUser(id)));

            var user = this.usersRepository.Read(id);
            Assert.NotNull(this.usersRepository.Read(id));

            user.Name = "Name2";

            Assert.True(this.usersRepository.Update(user));

            user = this.usersRepository.Read(id);

            Assert.AreEqual("Name2", user.Name);
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.Throws<NotSupportedException>(() => this.usersRepository.Delete("fake_id"));
        }

        /// <summary>
        /// Test accept invite.
        /// </summary>
        [Test]
        public void AcceptInvite()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());

            user1.FriendRequests.Add(user2.GetSynopsis());

            Assert.True(this.usersRepository.Create(user1));
            Assert.True(this.usersRepository.Create(user2));

            Assert.True(this.usersRepository.AcceptInvite(user2.Id, user1.Id));

            user1 = this.usersRepository.Read(user1.Id);
            user2 = this.usersRepository.Read(user2.Id);

            Assert.True(user1.Friends.Any(synopsis => synopsis.Id.Equals(user2.Id)));
            Assert.True(user2.Friends.Any(synopsis => synopsis.Id.Equals(user1.Id)));
        }

        /// <summary>
        /// Test add subscription.
        /// </summary>
        [Test]
        public void AddSubscription()
        {
            var id = Utils.CreateId();

            var tvshow = Utils.CreateTvShow(Utils.CreateId());

            Assert.True(this.usersRepository.Create(Utils.CreateUser(id)));

            var subscription = new Subscription { TvShow = tvshow.GetSynopsis() };

            Assert.True(this.usersRepository.AddSubscription(id, subscription));

            var user = this.usersRepository.Read(id);

            Assert.True(user.Subscriptions.Any(sub => sub.TvShow.Id.Equals(tvshow.Id)));
        }

        /// <summary>
        /// Test add subscription.
        /// </summary>
        [Test]
        public void RemoveSubscription()
        {
            var user = Utils.CreateUser(Utils.CreateId());
            var tvshow = Utils.CreateTvShow(Utils.CreateId());
            var subscription = new Subscription { TvShow = tvshow.GetSynopsis() };

            user.Subscriptions.Add(subscription);

            Assert.True(this.usersRepository.Create(user));
            Assert.True(user.Subscriptions.Any(sub => sub.TvShow.Id.Equals(tvshow.Id)));

            Assert.True(this.usersRepository.RemoveSubscription(user.Id, subscription));

            user = this.usersRepository.Read(user.Id);

            Assert.False(user.Subscriptions.Any(sub => sub.TvShow.Id.Equals(tvshow.Id)));
        }

        /// <summary>
        /// Test add watched episode.
        /// </summary>
        [Test]
        public void AddWatchedEpisode()
        {
            var tvshow = Utils.CreateTvShow(Utils.CreateId());
            var user = Utils.CreateUser(Utils.CreateId());
            var subscription = new Subscription { TvShow = tvshow.GetSynopsis() };
            var episode = Utils.CreateEpisode(tvshow.Id, 1, 1);

            user.Subscriptions.Add(subscription);

            Assert.True(this.usersRepository.Create(user));
            Assert.True(this.usersRepository.AddWatchedEpisode(user.Id, episode.GetSynopsis()));

            user = this.usersRepository.Read(user.Id);

            Assert.True(
                user.Subscriptions.Any(
                    sub => sub.TvShow.Id.Equals(tvshow.Id) && 
                           sub.EpisodesWatched.Any(epi => epi.Id.TvShowId.Equals(tvshow.Id) && 
                                                          epi.Id.SeasonNumber == 1 && 
                                                          epi.Id.EpisodeNumber == 1)));
        }

        /// <summary>
        /// Test remove watched episode.
        /// </summary>
        [Test]
        public void RemoveWatchedEpisode()
        {
            var tvshow = Utils.CreateTvShow(Utils.CreateId());
            var user = Utils.CreateUser(Utils.CreateId());
            var subscription = new Subscription { TvShow = tvshow.GetSynopsis() };
            var episode = Utils.CreateEpisode(tvshow.Id, 1, 1);

            subscription.EpisodesWatched.Add(episode.GetSynopsis());
            user.Subscriptions.Add(subscription);

            Assert.True(this.usersRepository.Create(user));

            Assert.True(this.usersRepository.RemoveWatchedEpisode(user.Id, episode.GetSynopsis()));

            user = this.usersRepository.Read(user.Id);

            Assert.False(
                user.Subscriptions.Any(
                    sub => sub.TvShow.Id.Equals(tvshow.Id) &&
                           sub.EpisodesWatched.Any(epi => epi.Id.TvShowId.Equals(tvshow.Id) &&
                                                          epi.Id.SeasonNumber == 1 &&
                                                          epi.Id.EpisodeNumber == 1)));
        }

        /// <summary>
        /// Test invite friend.
        /// </summary>
        [Test]
        public void InviteFriend()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());

            Assert.True(this.usersRepository.Create(user1));
            Assert.True(this.usersRepository.Create(user2));

            Assert.True(this.usersRepository.InviteFriend(user1.Id, user2.Id));

            user2 = this.usersRepository.Read(user2.Id);

            Assert.True(user2.FriendRequests.Any(fr => fr.Id.Equals(user1.Id)));
        }

        /// <summary>
        /// Test reject invite.
        /// </summary>
        [Test]
        public void RejectInvite()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());

            user1.FriendRequests.Add(user2.GetSynopsis());

            Assert.True(this.usersRepository.Create(user1));
            Assert.True(this.usersRepository.Create(user2));

            Assert.True(this.usersRepository.RejectInvite(user2.Id, user1.Id));

            user1 = this.usersRepository.Read(user1.Id);

            Assert.False(user1.FriendRequests.Any(fr => fr.Id.Equals(user2.Id)));
            Assert.False(user1.Friends.Any(friend => friend.Id.Equals(user2.Id)));
        }

        /// <summary>
        /// Test remove friend.
        /// </summary>
        [Test]
        public void RemoveFriend()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());

            user1.Friends.Add(user2.GetSynopsis());
            user2.Friends.Add(user1.GetSynopsis());

            Assert.True(this.usersRepository.Create(user1));
            Assert.True(this.usersRepository.Create(user2));

            Assert.True(this.usersRepository.RemoveFriend(user1.Id, user2.Id));

            user1 = this.usersRepository.Read(user1.Id);
            user2 = this.usersRepository.Read(user2.Id);

            Assert.False(user1.Friends.Any(friend => friend.Id.Equals(user2.Id)));
            Assert.False(user2.Friends.Any(friend => friend.Id.Equals(user1.Id)));
        }

        /// <summary>
        /// Test remove television show suggestions.
        /// </summary>
        [Test]
        public void RemoveTvShowSuggestions()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());
            var tvshow = Utils.CreateTvShow(Utils.CreateId());

            user1.Suggestions.Add(new Suggestion{TvShow = tvshow.GetSynopsis(), User = user2.GetSynopsis()});

            Assert.True(this.usersRepository.Create(user1));
            Assert.True(this.usersRepository.RemoveTvShowSuggestions(user1.Id, tvshow.Id));

            user1 = this.usersRepository.Read(user1.Id);
            Assert.False(user1.Suggestions.Any(suggestion => suggestion.TvShow.Id.Equals(tvshow.Id)));
        }

        /// <summary>
        /// Test send suggestion.
        /// </summary>
        [Test]
        public void SendSuggestion()
        {
            var user1 = Utils.CreateUser(Utils.CreateId());
            var user2 = Utils.CreateUser(Utils.CreateId());
            var tvshow = Utils.CreateTvShow(Utils.CreateId());

            Assert.True(this.usersRepository.Create(user1));

            Assert.True(this.usersRepository.SendSuggestion(user1.Id, new Suggestion { TvShow = tvshow.GetSynopsis(), User = user2.GetSynopsis() }));

            user1 = this.usersRepository.Read(user1.Id);

            Assert.True(user1.Suggestions.Any(suggestion => suggestion.TvShow.Id.Equals(tvshow.Id) && suggestion.User.Id.Equals(user2.Id)));
        }
    }
}
