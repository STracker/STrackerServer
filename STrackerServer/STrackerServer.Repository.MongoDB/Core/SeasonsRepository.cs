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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

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
        /// Initializes a new instance of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        /// <param name="tvshowsRepository">
        /// Television shows repository.
        /// </param>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public SeasonsRepository(ITvShowsRepository tvshowsRepository, MongoClient client, MongoUrl url) 
            : base(client, url)
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
        /// Needs to create also the object synopse in television show seasons list.
        public override bool Create(Season entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            // Add the synopse of the entity to television show.
            var tvshow = this.tvshowsRepository.Read(entity.TvShowId);
            tvshow.SeasonSynopses.Add(entity.GetSynopsis());

            return collection.Insert(entity).Ok && this.tvshowsRepository.Update(tvshow);
        }

        /// <summary>
        /// Get one season.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public override Season Read(Tuple<string, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, key.Item1), Query<Season>.EQ(s => s.SeasonNumber, key.Item2));

            var season = collection.FindOneAs<Season>(query);

            season.Key = key;

            return season;
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
        /// Dont need to update the object synopse in television show seasons list because
        /// the synopse object holds only the season number and this number is never changed.
        public override bool Update(Season entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            return collection.Save(entity).Ok;
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
        /// Needs to delete also the object synopse in television show seasons list.
        public override bool Delete(Tuple<string, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            // Remove  the object synopse.
            var tvshow = this.tvshowsRepository.Read(key.Item1);

            var synopse = tvshow.SeasonSynopses.Find(s => s.SeasonNumber == key.Item2);

            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, key.Item1), Query<Season>.EQ(s => s.SeasonNumber, key.Item2));

            if (synopse == null)
            {
                return collection.FindAndRemove(query, SortBy.Null).Ok;
            }

            tvshow.SeasonSynopses.Remove(synopse);

            // In this case first remove the object synopse than remove the season, because can not have
            // one synopse for one season that not exists.
            return this.tvshowsRepository.Update(tvshow) && collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
