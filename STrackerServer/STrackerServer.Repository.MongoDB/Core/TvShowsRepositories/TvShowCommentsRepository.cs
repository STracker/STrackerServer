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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

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
        public TvShowCommentsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
        }

        /// <summary>
        /// The add comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddComment(string key, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix));
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, key);
            var update = Update<CommentsTvShow>.Push(c => c.Comments, comment);

            return this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// The remove comment.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveComment(string key, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix));
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, key);
            var update = Update<CommentsTvShow>.Pull(c => c.Comments, comment);

            return this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(CommentsTvShow entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix));
            this.SetupIndexes(collection);
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
            var query = Query<CommentsTvShow>.EQ(c => c.TvShowId, id);
            return collection.FindOne<CommentsTvShow>(query, "_id", "Key");
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(CommentsTvShow entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<CommentsTvShow>.EQ(c => c.TvShowId, id);

            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}