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
        /// The container type.
        /// </summary>
        private const string ContainerType = "Comments";

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
            return this.Database.GetCollection(entity.Key).Insert(entity).Ok;
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
            var query = Query.And(
                Query<TvShowComments>.EQ(comments => comments.Key, key),
                Query<TvShowComments>.EQ(comments => comments.ContainerType, ContainerType));
            return this.Database.GetCollection(key).FindOneAs<TvShowComments>(query);
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
            var query = Query.And(
                 Query<TvShowComments>.EQ(c => c.Key, entity.Key),
                 Query<TvShowComments>.EQ(c => c.ContainerType, ContainerType));

            var update = Update<TvShowComments>.Set(showComments => showComments.Items, entity.Items);
            return this.Database.GetCollection(entity.Key).Update(query, update).Ok;
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
            var query = Query.And(
                 Query<TvShowComments>.EQ(c => c.Key, key),
                 Query<TvShowComments>.EQ(c => c.ContainerType, ContainerType));

            return this.Database.GetCollection(key).FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
