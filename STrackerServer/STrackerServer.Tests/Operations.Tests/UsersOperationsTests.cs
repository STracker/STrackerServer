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

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.BusinessLayer.Operations;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Repository.MongoDB.Core;

    /// <summary>
    /// The users operations tests.
    /// </summary>
    [TestFixture]
    public class UsersOperationsTests
    {
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
            this.client = new MongoClient();
            this.url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);

            this.userRepo = new UsersRepository(this.client, this.url);
            this.tvshowRepo = new TvShowsRepository(this.client, this.url);

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
        public void Create()
        {
            User user = new User
                {
                    Email = "fake email",
                    Friends =
                        new List<User.UserSynopsis>
                            { new User.UserSynopsis { Id = "0" }, new User.UserSynopsis { Id = "1" } },
                    Key = "fake key",
                    Name = "fake name",
                    Photo = new Artwork { ImageUrl = "fake image url" },
                    SubscriptionList =
                        new List<TvShow.TvShowSynopsis>
                            {
                                new TvShow.TvShowSynopsis { Id = "0", Name = "tvshow 0" },
                                new TvShow.TvShowSynopsis { Id = "1", Name = "tvshow 1" }
                            }
                };

            Assert.True(this.userOps.Create(user));
            Assert.False(this.userOps.Create(user));
        }

        /// <summary>
        /// The read test.
        /// </summary>
        [Test]
        public void Read()
        {
            User user1 = new User
                {
                    Email = "fake email",
                    Friends =
                        new List<User.UserSynopsis>
                                { 
                                    new User.UserSynopsis { Id = "0" },
                                    new User.UserSynopsis { Id = "1" } 
                                },
                    Key = "fake key",
                    Name = "fake name",
                    Photo = new Artwork { ImageUrl = "fake image url" },
                    SubscriptionList =
                        new List<TvShow.TvShowSynopsis>
                                {
                                    new TvShow.TvShowSynopsis { Id = "0", Name = "tvshow 0" },
                                    new TvShow.TvShowSynopsis { Id = "1", Name = "tvshow 1" }
                                }
                };

            User user2 = this.userOps.Read("fake key");

            // Compare
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

            for (int i = 0; i < user1.Friends.Count; i++)
            {
                Assert.AreEqual(user1.Friends[i].Id, user2.Friends[i].Id);
            }
        }

        /// <summary>
        /// The update test.
        /// </summary>
        [Test]
        public void Update()
        {
            User user1 = this.userOps.Read("fake key");
            user1.Name = "new fake name";

            Assert.True(this.userOps.Update(user1));

            User user2 = this.userOps.Read("fake key");

            Assert.AreEqual(user1.Name, user2.Name);
        }

        /// <summary>
        /// The delete test.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.True(this.userOps.Delete("fake key"));
            Assert.False(this.userOps.Delete("fake key"));
        }
    }
}
