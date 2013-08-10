// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show comments repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System;
    using System.Collections.Generic;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The television show comments repository.
    /// </summary>
    public class TvShowCommentsRepository : BaseCommentsRepository<CommentsTvShow, string>, ITvShowCommentsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowCommentsRepository"/> class.
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
        public TvShowCommentsRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
        }

        /// <summary>
        /// Add one comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddComment(string id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<CommentsTvShow>.EQ(comments => comments.Id, id);

            // Using Push insted of AddToSet because one user can write two or more equals comments.
            var update = Update<CommentsTvShow>.Push(c => c.Comments, comment);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Remove one comment.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveComment(string id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<CommentsTvShow>.EQ(comments => comments.Id, id);
            var update = Update<CommentsTvShow>.Pull(c => c.Comments, comment);

            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(CommentsTvShow entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id, CollectionPrefix));
            collection.Insert(entity);
        }

        /// <summary>
        /// The hook read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CommentsTvShow"/>.
        /// </returns>
        protected override CommentsTvShow HookRead(string id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            return collection.FindOneByIdAs<CommentsTvShow>(id);
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(CommentsTvShow entity)
        {
            // Nothing to do...
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
        protected override ICollection<CommentsTvShow> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}