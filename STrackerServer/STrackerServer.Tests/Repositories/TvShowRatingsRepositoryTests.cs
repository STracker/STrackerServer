﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRatingsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowRatingsRepositoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television show ratings repository.
    /// </summary>
    [TestFixture]
    public class TvShowRatingsRepositoryTests
    {
        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// The television show ratings repository.
        /// </summary>
        private readonly ITvShowRatingsRepository tvshowRatingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowRatingsRepositoryTests"/> class.
        /// </summary>
        public TvShowRatingsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowsRepository = kernel.Get<ITvShowsRepository>();
            this.tvshowRatingsRepository = kernel.Get<ITvShowRatingsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var tvshowRatings = new RatingsTvShow(Utils.CreateId());
            Assert.True(this.tvshowRatingsRepository.Create(tvshowRatings));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var tvshowRatings = new RatingsTvShow(Utils.CreateId());
            Assert.True(this.tvshowRatingsRepository.Create(tvshowRatings));
            Assert.False(this.tvshowRatingsRepository.Create(tvshowRatings));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var tvshowRatings = new RatingsTvShow(Utils.CreateId());
            Assert.True(this.tvshowRatingsRepository.Create(tvshowRatings));
            Assert.NotNull(this.tvshowRatingsRepository.Read(tvshowRatings.Id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.tvshowRatingsRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowRatingsRepository.ReadAll());
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            var tvshowRatings = new RatingsTvShow(Utils.CreateId());
            var user = Utils.CreateUser(Utils.CreateId());

            Assert.True(this.tvshowRatingsRepository.Create(tvshowRatings));

            tvshowRatings.Ratings.Add(new Rating { User = user.GetSynopsis(), UserRating = 5 });

            Assert.True(this.tvshowRatingsRepository.Update(tvshowRatings));

            tvshowRatings = this.tvshowRatingsRepository.Read(tvshowRatings.Id);

            Assert.AreEqual(5, tvshowRatings.Average);
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowRatingsRepository.Delete("fake_id"));
        }

        /// <summary>
        /// Test read top rated.
        /// </summary>
        [Test]
        public void ReadTopRated()
        {
            var user = Utils.CreateUser("fake_id");

            var id1 = Utils.CreateId();
            var id2 = Utils.CreateId();
            var id3 = Utils.CreateId();

            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id1)));
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id2)));
            Assert.True(this.tvshowsRepository.Create(Utils.CreateTvShow(id3)));

            Assert.True(this.tvshowRatingsRepository.AddRating(id1, new Rating { User = user.GetSynopsis(), UserRating = 5 }));
            Assert.True(this.tvshowRatingsRepository.AddRating(id2, new Rating { User = user.GetSynopsis(), UserRating = 4 }));
            Assert.True(this.tvshowRatingsRepository.AddRating(id3, new Rating { User = user.GetSynopsis(), UserRating = 3 }));

            var toprated = this.tvshowRatingsRepository.ReadTopRated(2);

            Assert.AreEqual(2, toprated.Count);

            Assert.AreEqual(id1, toprated.ElementAt(0).Id);
            Assert.AreEqual(id2, toprated.ElementAt(1).Id);
        }
    }
}
