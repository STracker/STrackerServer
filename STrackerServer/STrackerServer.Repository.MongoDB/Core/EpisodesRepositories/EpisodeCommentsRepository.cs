// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodeCommentsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The episode comments repository.
    /// </summary>
    public class EpisodeCommentsRepository : BaseCommentsRepository<CommentsEpisode, Episode.EpisodeKey>, IEpisodeCommentsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeCommentsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        public EpisodeCommentsRepository(MongoClient client, MongoUrl url)
            : base(client, url)
        {
        }

        /// <summary>
        /// The add comment.
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
        public override bool AddComment(Episode.EpisodeKey id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<CommentsEpisode>.EQ(c => c.Id, id);
            var update = Update<CommentsEpisode>.Push(c => c.Comments, comment);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// The remove comment.
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
        public override bool RemoveComment(Episode.EpisodeKey id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<CommentsEpisode>.EQ(c => c.Id, id);
            var update = Update<CommentsEpisode>.Pull(c => c.Comments, comment);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(CommentsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id.TvshowId, CollectionPrefix));
            collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CommentsEpisode"/>.
        /// </returns>
        protected override CommentsEpisode HookRead(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            return collection.FindOneByIdAs<CommentsEpisode>(id.ToBsonDocument());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(CommentsEpisode entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvshowId, CollectionPrefix));
            var query = Query<CommentsEpisode>.EQ(c => c.Id, id);
            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}