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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

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

                    // ignoring _id field when deserialize.
                    cm.SetIgnoreExtraElementsIsInherited(true);
                    cm.SetIgnoreExtraElements(true);
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository, MongoClient client, MongoUrl url) 
            : base(client, url)
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
        /// Needs to create also the object synopse in season episodes list.
        public override bool Create(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            // Add the synopse of the entity to season.
            var season = this.seasonsRepository.Read(new Tuple<string, int>(entity.TvShowId, entity.SeasonNumber));
            var synopse = entity.GetSynopsis();

            season.EpisodeSynopses.Add(synopse);

            return collection.Insert(entity).Ok && this.seasonsRepository.Update(season);
        }

        /// <summary>
        /// Get one episode.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        public override Episode Read(Tuple<string, int, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, key.Item1), Query<Episode>.EQ(e => e.SeasonNumber, key.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, key.Item3));

            var episode = collection.FindOneAs<Episode>(query);

            episode.Key = key;

            return episode;
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
        /// Needs to update also the object synopse in season episodes list.
        public override bool Update(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            // Update the object synopse.
            var season = this.seasonsRepository.Read(new Tuple<string, int>(entity.TvShowId, entity.EpisodeNumber));

            var synopse = season.EpisodeSynopses.Find(e => e.EpisodeNumber == entity.EpisodeNumber);
           
            if (synopse == null)
            {
                return collection.Save(entity).Ok;
            }

            synopse.Name = entity.Name;

            return collection.Save(entity).Ok && this.seasonsRepository.Update(season);
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
        /// Needs to delete also the object synopse in season episodes list.
        public override bool Delete(Tuple<string, int, int> key)
        {
            var collection = Database.GetCollection(key.Item1);

            // Remove object synopse.
            var season = this.seasonsRepository.Read(new Tuple<string, int>(key.Item1, key.Item2));

            var synopse = season.EpisodeSynopses.Find(s => s.EpisodeNumber == key.Item3);

            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, key.Item1), Query<Episode>.EQ(e => e.SeasonNumber, key.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, key.Item3));

            if (synopse == null)
            {
                return collection.FindAndRemove(query, SortBy.Null).Ok;
            }

            season.EpisodeSynopses.Remove(synopse);

            // In this case first remove the object synopse than remove the episode, because can not have
            // one synopse for one episode that not exists.
            return this.seasonsRepository.Update(season) && collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
