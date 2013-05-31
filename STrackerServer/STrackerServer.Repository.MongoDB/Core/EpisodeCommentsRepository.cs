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
        /// The container type.
        /// </summary>
        private const string ContainerType = "Comments";

        /// <summary>
        /// Initializes static members of the <see cref="EpisodeCommentsRepository"/> class.
        /// </summary>
        static EpisodeCommentsRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Container<Tuple<string, int, int>, Comment>)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<Container<Tuple<string, int, int>, Comment>>(
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
            return this.Database.GetCollection(entity.Key.Item1).Insert(entity).Ok;
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
                Query<EpisodeComments>.EQ(comments => comments.Key.Item3, key.Item3),
                Query<EpisodeComments>.EQ(comments => comments.ContainerType, ContainerType));

            var comment = this.Database.GetCollection(key.Item1).FindOneAs<EpisodeComments>(query);

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
                Query<EpisodeComments>.EQ(c => c.Key.Item3, entity.Key.Item3),
                Query<EpisodeComments>.EQ(c => c.ContainerType, ContainerType));

            var update = Update<EpisodeComments>.Set(showComments => showComments.Items, entity.Items);

            return this.Database.GetCollection(entity.Key.Item1).Update(query, update).Ok;
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
                Query<EpisodeComments>.EQ(c => c.Key.Item3, key.Item3),
                Query<EpisodeComments>.EQ(c => c.ContainerType, ContainerType));

            return this.Database.GetCollection(key.Item1).FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
