// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsRepository interface. This repository connects with MongoDB 
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
    /// Seasons repository for MongoDB database.
    /// </summary>
    public class SeasonsRepository : BaseRepository<Season, Tuple<string, int>>, ISeasonsRepository
    {
        /// <summary>
        /// Television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

        /// <summary>
        /// Initializes static members of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        static SeasonsRepository()
        {
            BsonClassMap.RegisterClassMap<Season>(
                cm => cm.MapIdField(c => c.Key.ToString()));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        /// <param name="tvshowsRepository">
        /// Television shows repository.
        /// </param>
        public SeasonsRepository(ITvShowsRepository tvshowsRepository)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        /// <summary>
        /// Create one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Season entity)
        {
            // Needs to create also the object synopse in television show seasons list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Season entity)
        {
            // Needs to update also the object synopse in television show seasons list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete one season.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(Tuple<string, int> key)
        {
            // Needs to delete also the object synopse in television show seasons list
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        protected override Season HookRead(Tuple<string, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            return collection.FindOneByIdAs<Season>(key.ToString());
        }
    }
}
