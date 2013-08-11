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
    using System.Collections.Generic;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The episode comments repository.
    /// </summary>
    public class EpisodeCommentsRepository : BaseCommentsRepository<CommentsEpisode, Episode.EpisodeId>, IEpisodeCommentsRepository
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
        /// <param name="logger">
        /// The logger.
        /// </param>
        public EpisodeCommentsRepository(MongoClient client, MongoUrl url, ILogger logger)
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
        public bool AddComment(Episode.EpisodeId id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
            var query = Query<CommentsEpisode>.EQ(c => c.Id, id);
            var update = Update<CommentsEpisode>.Push(c => c.Comments, comment);
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
        public bool RemoveComment(Episode.EpisodeId id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
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
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.Id.TvShowId, CollectionPrefix));
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
        protected override CommentsEpisode HookRead(Episode.EpisodeId id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.TvShowId, CollectionPrefix));
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
        protected override void HookDelete(Episode.EpisodeId id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id, CollectionPrefix));
            var query = Query<CommentsEpisode>.EQ(c => c.Id, id);
            collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<CommentsEpisode> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}