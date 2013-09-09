// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentsRepositoryTests.cs" company="STracker">
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
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The television show ratings repository.
    /// </summary>
    [TestFixture]
    public class TvShowCommentsRepositoryTests
    {
        /// <summary>
        /// The television show comments repository.
        /// </summary>
        private readonly ITvShowCommentsRepository tvshowCommentsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowCommentsRepositoryTests"/> class.
        /// </summary>
        public TvShowCommentsRepositoryTests()
        {
            var kernel = new StandardKernel(new ModuleForUnitTests());
            this.tvshowCommentsRepository = kernel.Get<ITvShowCommentsRepository>();
        }

        /// <summary>
        /// Test create.
        /// </summary>
        [Test]
        public void Create()
        {
            var tvshowComments = new CommentsTvShow(Utils.CreateId());
            Assert.True(this.tvshowCommentsRepository.Create(tvshowComments));
        }

        /// <summary>
        /// Test create fail.
        /// </summary>
        [Test]
        public void CreateFail()
        {
            var tvshowComments = new CommentsTvShow(Utils.CreateId());
            Assert.True(this.tvshowCommentsRepository.Create(tvshowComments));
            Assert.False(this.tvshowCommentsRepository.Create(tvshowComments));
        }

        /// <summary>
        /// Test read.
        /// </summary>
        [Test]
        public void Read()
        {
            var tvshowComments = new CommentsTvShow(Utils.CreateId());
            Assert.True(this.tvshowCommentsRepository.Create(tvshowComments));
            Assert.NotNull(this.tvshowCommentsRepository.Read(tvshowComments.Id));
        }

        /// <summary>
        /// Test read fail.
        /// </summary>
        [Test]
        public void ReadFail()
        {
            Assert.Null(this.tvshowCommentsRepository.Read("fake_id"));
        }

        /// <summary>
        /// Test read all.
        /// </summary>
        [Test]
        public void ReadAll()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowCommentsRepository.ReadAll());
        }

        /// <summary>
        /// Test update.
        /// </summary>
        [Test]
        public void Update()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowCommentsRepository.Update(new CommentsTvShow("fake_id")));
        }

        /// <summary>
        /// Test delete.
        /// </summary>
        [Test]
        public void Delete()
        {
            Assert.Throws<NotSupportedException>(() => this.tvshowCommentsRepository.Delete("fake_id"));
        }
  
        /// <summary>
        /// Test add comment.
        /// </summary>
        [Test]
        public void AddComment()
        {
            var tvshowComments = new CommentsTvShow(Utils.CreateId());
            var user = Utils.CreateUser(Utils.CreateId());
            var comment = new Comment { Id = Utils.CreateId(), User = user.GetSynopsis(), Body = "Body" };

            Assert.True(this.tvshowCommentsRepository.Create(tvshowComments));

            Assert.True(this.tvshowCommentsRepository.AddComment(tvshowComments.Id, comment));

            tvshowComments = this.tvshowCommentsRepository.Read(tvshowComments.Id);

            Assert.True(tvshowComments.Comments.Any(com => com.User.Id.Equals(user.Id) && com.Id.Equals(comment.Id)));
        }

        /// <summary>
        /// Test remove comment.
        /// </summary>
        [Test]
        public void RemoveComment()
        {
            var tvshowComments = new CommentsTvShow(Utils.CreateId());
            var user = Utils.CreateUser(Utils.CreateId());
            var comment = new Comment { Id = Utils.CreateId(), User = user.GetSynopsis(), Body = "Body" };

            tvshowComments.Comments.Add(comment);

            Assert.True(this.tvshowCommentsRepository.Create(tvshowComments));

            this.tvshowCommentsRepository.RemoveComment(tvshowComments.Id, comment);

            tvshowComments = this.tvshowCommentsRepository.Read(tvshowComments.Id);

            Assert.False(tvshowComments.Comments.Any(com => com.User.Id.Equals(user.Id) && com.Id.Equals(comment.Id)));
        }
    }
}
