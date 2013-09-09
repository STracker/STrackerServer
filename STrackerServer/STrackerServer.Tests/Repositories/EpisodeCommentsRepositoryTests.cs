// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeCommentsRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode comments repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The episode comments repository tests.
    /// </summary>
    [TestFixture]
    public class EpisodeCommentsRepositoryTests
    {
        /// <summary>
        /// The episodes comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository episodeCommentsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeCommentsRepositoryTests"/> class.
        /// </summary>
        public EpisodeCommentsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.episodeCommentsRepository = kernel.Get<IEpisodeCommentsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var episodesComments = new CommentsEpisode(new Episode.EpisodeId { TvShowId = Utils.CreateId(), SeasonNumber = 1, EpisodeNumber = 1 });
            Assert.True(this.episodeCommentsRepository.Create(episodesComments));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var episodesComments = new CommentsEpisode(new Episode.EpisodeId { TvShowId = Utils.CreateId(), SeasonNumber = 1, EpisodeNumber = 1 });
            Assert.True(this.episodeCommentsRepository.Create(episodesComments));
            Assert.False(this.episodeCommentsRepository.Create(episodesComments));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var episodesComments = new CommentsEpisode(new Episode.EpisodeId { TvShowId = Utils.CreateId(), SeasonNumber = 1, EpisodeNumber = 1 });
            Assert.True(this.episodeCommentsRepository.Create(episodesComments));
            Assert.NotNull(this.episodeCommentsRepository.Read(episodesComments.Id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.episodeCommentsRepository.Read(new Episode.EpisodeId()));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.episodeCommentsRepository.ReadAll());
        }
    }
}
