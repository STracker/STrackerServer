// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The television show comments repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show comments repository.
    /// </summary>
    public class TvShowCommentsRepository : BaseRepository<TvShowComments, string>, ITvShowCommentsRepository
    {
        /// <summary>
        /// The collection prefix.
        /// </summary>
        private const string CollectionPrefix = "Comments";

        /// <summary>
        /// Initializes static members of the <see cref="TvShowCommentsRepository"/> class.
        /// </summary>
        static TvShowCommentsRepository()
        {
             if (BsonClassMap.IsClassMapRegistered(typeof(TvShowComments)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<TvShowComments>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapProperty(c => c.Key);

                        // ignoring _id field when deserialize.
                        cm.SetIgnoreExtraElementsIsInherited(true);
                        cm.SetIgnoreExtraElements(true);
                    });
        }

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
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Is abstract because his implementation is diferent from repository to repository.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(TvShowComments entity)
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
        public override TvShowComments HookRead(string key)
        {
            var query = Query<TvShowComments>.EQ(comments => comments.TvShowId, key);

            var comment = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).FindOneAs<TvShowComments>(query);
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
        public override bool HookUpdate(TvShowComments entity)
        {
            var query = Query<TvShowComments>.EQ(comments => comments.TvShowId, entity.Key);
            var update = Update<TvShowComments>.Set(showComments => showComments.Comments, entity.Comments);

            return this.Database.GetCollection(string.Format("{0}-{1}", entity.TvShowId, CollectionPrefix)).Update(query, update).Ok;
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
        public override bool HookDelete(string key)
        {
            var query = Query<TvShowComments>.EQ(comments => comments.TvShowId, key);

            return this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}