// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersOperationsTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Unit tests for users operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Operations.Tests
{
    using System.Collections.Generic;
    using System.Configuration;

    using MongoDB.Driver;

    using NUnit.Framework;

    using STrackerServer.BusinessLayer.Core.UsersOperations;
    using STrackerServer.BusinessLayer.Operations.UsersOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.Core.UsersRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Repository.MongoDB.Core.TvShowsRepositories;
    using STrackerServer.Repository.MongoDB.Core.UsersRepositories;

    /// <summary>
    /// The users operations tests.
    /// </summary>
    [TestFixture]
    public class UsersOperationsTests
    {
        /*
        /// <summary>
        /// The mongo client.
        /// </summary>
        private MongoClient client;

        /// <summary>
        /// The mongo url.
        /// </summary>
        private MongoUrl url;

        /// <summary>
        /// The user repository.
        /// </summary>
        private IUsersRepository userRepo;

        /// <summary>
        /// The television show repository.
        /// </summary>
        private ITvShowsRepository tvshowRepo;

        /// <summary>
        /// The user operations.
        /// </summary>
        private IUsersOperations userOps;

        /// <summary>
        /// The set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);
            this.client = new MongoClient(this.url);
              
            this.userRepo = new UsersRepository(this.client, this.url);

            this.tvshowRepo = new TvShowsRepository(
                this.client, 
                this.url, 
                new GenresRepository(this.client, this.url), 
                new TvShowCommentsRepository(this.client, this.url),
                new TvShowRatingsRepository(this.client, this.url));

            this.userOps = new UsersOperations(this.userRepo, this.tvshowRepo);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
        }

        /// <summary>
        /// The create test.
        /// </summary>
        [Test]
        public void Test1Create()
        {
            var user = new User
                {
                    Email = "fake email",
                    Friends =
                        new List<User.UserSynopsis>
                            {
                                new User.UserSynopsis { Id = "0" }, new User.UserSynopsis { Id = "1" } 
                            },
                    Key = "fake key",
                    Name = "fake name",
                    Photo = new Artwork { ImageUrl = "fake image url" },
                    SubscriptionList =
                        new List<TvShow.TvShowSynopsis>
                            {
                                new TvShow.TvShowSynopsis { Id = "0" }, new TvShow.TvShowSynopsis { Id = "1" } 
                            },
                    FriendRequests = new List<User.UserSynopsis>(),
                    Suggestions = new List<Suggestion>()
                };

            Assert.True(this.userOps.Create(user));
            Assert.False(this.userOps.Create(user));
        }

        /// <summary>
        /// The read test.
        /// </summary>
        [Test]
        public void Test2Read()
        {
            var user1 = new User
                {
                    Email = "fake email",
                    Friends =
                        new List<User.UserSynopsis>
                            {
                                new User.UserSynopsis { Id = "0" }, new User.UserSynopsis { Id = "1" } 
                            },
                    Key = "fake key",
                    Name = "fake name",
                    Photo = new Artwork { ImageUrl = "fake image url" },
                    SubscriptionList =
                        new List<TvShow.TvShowSynopsis>
                            {
                                new TvShow.TvShowSynopsis { Id = "0" }, new TvShow.TvShowSynopsis { Id = "1" } 
                            },
                    FriendRequests = new List<User.UserSynopsis>(),
                    Suggestions = new List<Suggestion>()
                };

            var user2 = this.userOps.Read("fake key");

            Assert.AreEqual(user1.Email, user2.Email);

            if (user1.Friends.Count != user2.Friends.Count)
            {
                Assert.Fail("Friend lists have different number of items.");
            }

            for (int i = 0; i < user1.Friends.Count; i++)
            {
                Assert.AreEqual(user1.Friends[i].Id, user2.Friends[i].Id);
            }

            Assert.AreEqual(user1.Key, user2.Key);
            Assert.AreEqual(user1.Name, user2.Name);
            Assert.AreEqual(user1.Photo.ImageUrl, user2.Photo.ImageUrl);

            if (user1.SubscriptionList.Count != user2.SubscriptionList.Count)
            {
                Assert.Fail("Subscription lists have different number of items.");
            }

            for (int i = 0; i < user1.SubscriptionList.Count; i++)
            {
                Assert.AreEqual(user1.Friends[i].Id, user2.Friends[i].Id);
            }

            if (user1.Friends.Count != user2.Friends.Count)
            {
                Assert.Fail("Friends lists have different number of items.");
            }

            for (int i = 0; i < user1.Friends.Count; i++)
            {
                Assert.AreEqual(user1.Friends[i].Id, user2.Friends[i].Id);
            }
        }

        /// <summary>
        /// The delete test.
        /// </summary>
        [Test]
        public void Test3Delete()
        {
            Assert.NotNull(this.userOps.Read("fake key"));
            this.userOps.Delete("fake key");
            Assert.Null(this.userOps.Read("fake key"));
        }
         * */
    }
}
