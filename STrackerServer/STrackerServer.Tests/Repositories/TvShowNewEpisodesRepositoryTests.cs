// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowNewEpisodesRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowNewEpisodesRepositoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show new episodes repository tests.
    /// </summary>
    [TestFixture]
    public class TvShowNewEpisodesRepositoryTests
    {
        /// <summary>
        /// The new episodes repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository newEpisodesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowNewEpisodesRepositoryTests"/> class.
        /// </summary>
        public TvShowNewEpisodesRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.newEpisodesRepository = kernel.Get<ITvShowNewEpisodesRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));
            Assert.False(this.newEpisodesRepository.Create(newTvShowEpisodes));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));
            Assert.NotNull(this.newEpisodesRepository.Read(newTvShowEpisodes.Id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.newEpisodesRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            var newTvShowEpisodes1 = new NewTvShowEpisodes(Utils.CreateId());
            var newTvShowEpisodes2 = new NewTvShowEpisodes(Utils.CreateId());

            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes1));
            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes2));

            Assert.True(this.newEpisodesRepository.ReadAll().Count >= 2);
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId())
                                        {
                                            TvShow = new TvShow.TvShowSynopsis { Name = "Name" }
                                        };

            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));

            newTvShowEpisodes.TvShow.Name = "Name2";

            Assert.True(this.newEpisodesRepository.Update(newTvShowEpisodes));

            newTvShowEpisodes = this.newEpisodesRepository.Read(newTvShowEpisodes.Id);

            Assert.AreEqual("Name2", newTvShowEpisodes.TvShow.Name);
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));
            Assert.True(this.newEpisodesRepository.Delete(newTvShowEpisodes.Id));
            Assert.Null(this.newEpisodesRepository.Read(newTvShowEpisodes.Id));
        }

        /// <summary>
        /// Test add episode.
        /// </summary>
        [Test]
        public void AddEpisode()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            var episode = Utils.CreateEpisode(newTvShowEpisodes.Id, 1, 1);

            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));

            Assert.True(this.newEpisodesRepository.AddEpisode(newTvShowEpisodes.Id, episode.GetSynopsis()));

            newTvShowEpisodes = this.newEpisodesRepository.Read(newTvShowEpisodes.Id);

            Assert.True(newTvShowEpisodes.Episodes.Any(synopsis => synopsis.Id.SeasonNumber == 1 && synopsis.Id.EpisodeNumber == 1));
        }

        /// <summary>
        /// Test remove episode.
        /// </summary>
        [Test]
        public void RemoveEpisode()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());
            var episode = Utils.CreateEpisode(newTvShowEpisodes.Id, 1, 1);

            newTvShowEpisodes.Episodes.Add(episode.GetSynopsis());

            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));

            Assert.True(this.newEpisodesRepository.RemoveEpisode(newTvShowEpisodes.Id, episode.GetSynopsis()));

            newTvShowEpisodes = this.newEpisodesRepository.Read(newTvShowEpisodes.Id);

            Assert.False(newTvShowEpisodes.Episodes.Any(synopsis => synopsis.Id.SeasonNumber == 1 && synopsis.Id.EpisodeNumber == 1));
        }

        /// <summary>
        /// Test delete old episodes.
        /// </summary>
        [Test]
        public void DeleteOldEpisodes()
        {
            var newTvShowEpisodes = new NewTvShowEpisodes(Utils.CreateId());

            var episode = Utils.CreateEpisode(newTvShowEpisodes.Id, 1, 1);
            episode.Date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            newTvShowEpisodes.Episodes.Add(episode.GetSynopsis());

            Assert.True(this.newEpisodesRepository.Create(newTvShowEpisodes));

            this.newEpisodesRepository.DeleteOldEpisodes();

            newTvShowEpisodes = this.newEpisodesRepository.Read(newTvShowEpisodes.Id);

            Assert.False(newTvShowEpisodes.Episodes.Any(synopsis => synopsis.Id.SeasonNumber == 1 && synopsis.Id.EpisodeNumber == 1));
        }
    }
}
