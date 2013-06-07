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
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;

    using global::MongoDB.Bson.Serialization;
    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show comments repository.
    /// </summary>
    public class TvShowCommentsRepository : BaseRepository<CommentsTvShow, string>, ITvShowCommentsRepository
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
            if (BsonClassMap.IsClassMapRegistered(typeof(CommentsBase<string>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<CommentsBase<string>>(
                cm =>
                    {
                        cm.AutoMap();
                        cm.UnmapProperty(c => c.Key);

                        // ignoring _id field when deserialize.
                        cm.SetIgnoreExtraElementsIsInherited(true);
                        cm.SetIgnoreExtraElements(true);
                    });
            BsonClassMap.RegisterClassMap<CommentsTvShow>();
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
        public override bool HookCreate(CommentsTvShow entity)
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
        public override CommentsTvShow HookRead(string key)
        {
            var query = Query<CommentsTvShow>.EQ(c => c.TvShowId, key);

            var collection = this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix));

            var comments = collection.FindOneAs<CommentsTvShow>(query);
            if (comments == null)
            {
                return null;
            }

            comments.Key = key;

            return comments;
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
        public override bool HookUpdate(CommentsTvShow entity)
        {
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, entity.Key);
            var update = Update<CommentsTvShow>.Set(showComments => showComments.Comments, entity.Comments);

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
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, key);

            return this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).FindAndRemove(query, SortBy.Null).Ok;
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
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, key);

            return
                this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).Update(
                    query, Update<CommentsTvShow>.Push(tvc => tvc.Comments, comment)).Ok;
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
            var query = Query<CommentsTvShow>.EQ(comments => comments.TvShowId, key);

            return
                this.Database.GetCollection(string.Format("{0}-{1}", key, CollectionPrefix)).Update(
                    query, Update<CommentsTvShow>.Pull(tvc => tvc.Comments, comment)).Ok;
        }
    }
}