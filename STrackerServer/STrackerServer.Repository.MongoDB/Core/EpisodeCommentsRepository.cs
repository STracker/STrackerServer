// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the EpisodeCommentsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episode comments repository.
    /// </summary>
    public class EpisodeCommentsRepository : BaseRepository<CommentsEpisode, Tuple<string, int, int>>, IEpisodeCommentsRepository
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        private const string CollectionPrefix = "Comments";

        /// <summary>
        /// Initializes static members of the <see cref="EpisodeCommentsRepository"/> class.
        /// </summary>
        static EpisodeCommentsRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(CommentsBase<Tuple<string, int, int>>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<CommentsBase<Tuple<string, int, int>>>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapProperty(c => c.Key);

                        // ignoring _id field when deserialize.
                        cm.SetIgnoreExtraElementsIsInherited(true);
                        cm.SetIgnoreExtraElements(true);
                    });
            BsonClassMap.RegisterClassMap<CommentsEpisode>();
        }

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
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(CommentsEpisode entity)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix));

            // Ensure index.
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));

            return collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>T</cref>
        ///     </see> .
        /// </returns>
        public override CommentsEpisode HookRead(Tuple<string, int, int> key)
        {
            var query = Query.And(
                Query<CommentsEpisode>.EQ(comments => comments.Key.Item1, key.Item1),
                Query<CommentsEpisode>.EQ(comments => comments.Key.Item2, key.Item2),
                Query<CommentsEpisode>.EQ(comments => comments.Key.Item3, key.Item3));

            var comment = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix)).FindOneAs<CommentsEpisode>(query);
            if (comment == null)
            {
                return null;
            }

            comment.Key = key;

            return comment;
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(CommentsEpisode entity)
        {
            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.Key.Item1, entity.Key.Item1),
                Query<CommentsEpisode>.EQ(c => c.Key.Item2, entity.Key.Item2),
                Query<CommentsEpisode>.EQ(c => c.Key.Item3, entity.Key.Item3));

            var update = Update<CommentsEpisode>.Set(showComments => showComments.Comments, entity.Comments);

            return
                this.Database.GetCollection(string.Format("{0}-{1}", entity.Key.Item1, CollectionPrefix)).Update(
                    query, update).Ok;
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(Tuple<string, int, int> key)
        {
            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.Key.Item1, key.Item1),
                Query<CommentsEpisode>.EQ(c => c.Key.Item2, key.Item2),
                Query<CommentsEpisode>.EQ(c => c.Key.Item3, key.Item3));

            return
                this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix)).FindAndRemove(
                    query, SortBy.Null).Ok;
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
        public bool AddComment(Tuple<string, int, int> key, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.Key.Item1, key.Item1),
                Query<CommentsEpisode>.EQ(c => c.Key.Item2, key.Item2),
                Query<CommentsEpisode>.EQ(c => c.Key.Item3, key.Item3));

            return collection.Update(query, Update<CommentsEpisode>.Push(ec => ec.Comments, comment)).Ok;
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
        public bool RemoveComment(Tuple<string, int, int> key, Comment comment)
        {
            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix));

            var query = Query.And(
                Query<CommentsEpisode>.EQ(c => c.Key.Item1, key.Item1),
                Query<CommentsEpisode>.EQ(c => c.Key.Item2, key.Item2),
                Query<CommentsEpisode>.EQ(c => c.Key.Item3, key.Item3));

            return collection.Update(query, Update<CommentsEpisode>.Pull(ec => ec.Comments, comment)).Ok;
        }
    }
}