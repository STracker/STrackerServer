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

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episode comments repository.
    /// </summary>
    public class EpisodeCommentsRepository : BaseRepository<EpisodeComments, Tuple<string, int, int>>, IEpisodeCommentsRepository
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
            if (BsonClassMap.IsClassMapRegistered(typeof(BaseComments<Tuple<string, int, int>>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<BaseComments<Tuple<string, int, int>>>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapProperty(c => c.Key);

                        // ignoring _id field when deserialize.
                        cm.SetIgnoreExtraElementsIsInherited(true);
                        cm.SetIgnoreExtraElements(true);
                    });
            BsonClassMap.RegisterClassMap<EpisodeComments>();
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
        public override bool HookCreate(EpisodeComments entity)
        {
            return
                this.Database.GetCollection(string.Format("{0}-{1}", entity.Key.Item1, CollectionPrefix)).Insert(entity)
                    .Ok;
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
        public override EpisodeComments HookRead(Tuple<string, int, int> key)
        {
            var query = Query.And(
                Query<EpisodeComments>.EQ(comments => comments.Key.Item1, key.Item1),
                Query<EpisodeComments>.EQ(comments => comments.Key.Item2, key.Item2),
                Query<EpisodeComments>.EQ(comments => comments.Key.Item3, key.Item3));

            var comment = this.Database.GetCollection(string.Format("{0}-{1}", key.Item1, CollectionPrefix)).FindOneAs<EpisodeComments>(query);
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
        public override bool HookUpdate(EpisodeComments entity)
        {
            var query = Query.And(
                Query<EpisodeComments>.EQ(c => c.Key.Item1, entity.Key.Item1),
                Query<EpisodeComments>.EQ(c => c.Key.Item2, entity.Key.Item2),
                Query<EpisodeComments>.EQ(c => c.Key.Item3, entity.Key.Item3));

            var update = Update<EpisodeComments>.Set(showComments => showComments.Comments, entity.Comments);

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
                Query<EpisodeComments>.EQ(c => c.Key.Item1, key.Item1),
                Query<EpisodeComments>.EQ(c => c.Key.Item2, key.Item2),
                Query<EpisodeComments>.EQ(c => c.Key.Item3, key.Item3));

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
                Query<EpisodeComments>.EQ(c => c.Key.Item1, key.Item1),
                Query<EpisodeComments>.EQ(c => c.Key.Item2, key.Item2),
                Query<EpisodeComments>.EQ(c => c.Key.Item3, key.Item3));

            return collection.Update(query, Update<EpisodeComments>.Push(ec => ec.Comments, comment)).Ok;
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
                Query<EpisodeComments>.EQ(c => c.Key.Item1, key.Item1),
                Query<EpisodeComments>.EQ(c => c.Key.Item2, key.Item2),
                Query<EpisodeComments>.EQ(c => c.Key.Item3, key.Item3));

            return collection.Update(query, Update<EpisodeComments>.Pull(ec => ec.Comments, comment)).Ok;
        }
    }
}