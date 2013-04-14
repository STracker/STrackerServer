// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;

    using global::MongoDB.Bson.Serialization;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes repository for MongoDB database.
    /// </summary>
    public class EpisodesRepository : BaseRepository<Episode, Tuple<string, int, int>>, IEpisodesRepository
    {
        /// <summary>
        /// Seasons repository.
        /// </summary>
        private readonly ISeasonsRepository seasonsRepository;

        /// <summary>
        /// Initializes static members of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        static EpisodesRepository()
        {
            BsonClassMap.RegisterClassMap<Episode>(
                cm =>
                {
                    cm.AutoMap();
                    cm.UnmapProperty(c => c.Key);
                    cm.GetMemberMap(c => c.GuestActors).SetIgnoreIfNull(true);
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository)
        {
            this.seasonsRepository = seasonsRepository;
        }

        /// <summary>
        /// Create one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Episode entity)
        {
            // Needs to create also the object synopse in season episodes list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Episode entity)
        {
            // Needs to update also the object synopse in season episodes list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete one episode.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(Tuple<string, int, int> key)
        {
            // Needs to delete also the object synopse in season episodes list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        protected override Episode HookRead(Tuple<string, int, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            return collection.FindOneByIdAs<Episode>(key.ToString());
        }
    }
}
