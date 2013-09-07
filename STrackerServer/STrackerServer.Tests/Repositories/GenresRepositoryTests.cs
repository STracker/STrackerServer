// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresRepositoryTests.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genres repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Repositories
{
    using System;
    using System.Linq;

    using Ninject;

    using NUnit.Framework;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The genres repository tests.
    /// </summary>
    [TestFixture]
    public class GenresRepositoryTests
    {
        /// <summary>
        /// The genres repository.
        /// </summary>
        private readonly IGenresRepository genresRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenresRepositoryTests"/> class.
        /// </summary>
        public GenresRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.genresRepository = kernel.Get<IGenresRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            Assert.True(this.genresRepository.Create(Utils.CreateGenre(Utils.CreateId())));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var id = Utils.CreateId();

            Assert.True(this.genresRepository.Create(Utils.CreateGenre(id)));
            Assert.False(this.genresRepository.Create(Utils.CreateGenre(id)));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var id = Utils.CreateId();

            Assert.True(this.genresRepository.Create(Utils.CreateGenre(id)));
            Assert.NotNull(this.genresRepository.Read(id));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.genresRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test read all synopsis.
        /// </summary>
        [Test]
        public void ReadAllSynopsis()
        {
            Assert.True(this.genresRepository.Create(Utils.CreateGenre(Utils.CreateId())));
            Assert.True(this.genresRepository.Create(Utils.CreateGenre(Utils.CreateId())));
            Assert.True(this.genresRepository.Create(Utils.CreateGenre(Utils.CreateId())));

            Assert.True(this.genresRepository.ReadAllSynopsis().Count >= 3);
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.genresRepository.ReadAll());
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            Assert.Throws<NotSupportedException>(() => this.genresRepository.Update(new Genre()));
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.Throws<NotSupportedException>(() => this.genresRepository.Delete("fake_id"));
        }

        /// <summary>
        /// Test add and remove television show.
        /// </summary>
        [Test]
        public void AddTvShow()
        {
            var genre = Utils.CreateGenre(Utils.CreateId());
            var tvshow = Utils.CreateTvShow(Utils.CreateId());

            Assert.True(this.genresRepository.Create(Utils.CreateGenre(genre.Id)));

            Assert.True(this.genresRepository.AddTvShow(genre.Id, tvshow.GetSynopsis()));

            genre = this.genresRepository.Read(genre.Id);

            Assert.True(genre.Tvshows.Any(synopsis => synopsis.Id.Equals(tvshow.Id)));
        }

        /// <summary>
        /// Test remove television show.
        /// </summary>
        [Test]
        public void RemoveTvShow()
        {
            var tvshow = Utils.CreateTvShow(Utils.CreateId());

            var genre = Utils.CreateGenre(Utils.CreateId());
            genre.Tvshows.Add(tvshow.GetSynopsis());

            Assert.True(this.genresRepository.Create(genre));

            Assert.True(this.genresRepository.RemoveTvShow(genre.Id, tvshow.GetSynopsis()));

            genre = this.genresRepository.Read(genre.Id);

            Assert.False(genre.Tvshows.Any(synopsis => synopsis.Id.Equals(tvshow.Id)));
        }
    }
}
