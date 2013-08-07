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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    /// <summary>
    /// The episode comments repository.
    /// </summary>
    public class EpisodeCommentsRepository : BaseCommentsRepository<CommentsEpisode, Tuple<string, int, int>>, IEpisodeCommentsRepository
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
        public bool AddComment(Tuple<string, int, int> id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.TvShowId, id.Item1),
                Query<CommentsEpisode>.EQ(c => c.SeasonNumber, id.Item2),
                Query<CommentsEpisode>.EQ(c => c.EpisodeNumber, id.Item3));

            var update = Update<CommentsEpisode>.Push(c => c.Comments, comment);

            return this.ModifyList(collection, query, update);
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
        public bool RemoveComment(Tuple<string, int, int> id, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.TvShowId, id.Item1),
                Query<CommentsEpisode>.EQ(c => c.SeasonNumber, id.Item2),
                Query<CommentsEpisode>.EQ(c => c.EpisodeNumber, id.Item3));

            var update = Update<CommentsEpisode>.Pull(c => c.Comments, comment);

            return this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(CommentsEpisode entity)
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
        /// The <see cref="CommentsEpisode"/>.
        /// </returns>
        protected override CommentsEpisode HookRead(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(comments => comments.TvShowId, id.Item1),
                Query<CommentsEpisode>.EQ(comments => comments.SeasonNumber, id.Item2),
                Query<CommentsEpisode>.EQ(comments => comments.EpisodeNumber, id.Item3));

            return collection.FindOne<CommentsEpisode>(query, "_id");
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
        protected override void HookDelete(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", id.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.TvShowId, id.Item1),
                Query<CommentsEpisode>.EQ(c => c.SeasonNumber, id.Item2),
                Query<CommentsEpisode>.EQ(c => c.EpisodeNumber, id.Item3));

            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}