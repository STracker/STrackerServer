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
    using System.Collections.Generic;
    using System.Linq;

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
        /// Initializes static members of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        static SeasonsRepository()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Season)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<Season>(
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
        /// Needs to create also the object synopse in television show seasons list.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookCreate(Season entity)
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
        public override Season HookRead(Tuple<string, int> key)
        {
            var collection = Database.GetCollection(key.Item1);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, key.Item1), Query<Season>.EQ(s => s.SeasonNumber, key.Item2));

            var season = collection.FindOneAs<Season>(query);   
            if (season == null)
            {
                return null;
            }

            season.Key = key;
            return season;
        }

        /// <summary>
        /// Update one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Dont need to update the object synopse in television show seasons list because
        /// the synopse object holds only the season number and this number is never changed.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookUpdate(Season entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, entity.TvShowId), Query<Season>.EQ(s => s.SeasonNumber, entity.SeasonNumber));
            var update = Update<Season>.Set(s => s.EpisodeSynopses, entity.EpisodeSynopses);

            return collection.Update(query, update).Ok;
        }

        /// <summary>
        /// Delete one season.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// Needs to delete also the object synopse in television show seasons list.
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool HookDelete(Tuple<string, int> key)
        {
            var collection = Database.GetCollection(key.Item1);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, key.Item1), Query<Season>.EQ(s => s.SeasonNumber, key.Item2));

            // Remove  the object synopse.
            var tvshow = this.tvshowsRepository.Read(key.Item1);
            var synopse = tvshow.SeasonSynopses.Find(s => s.SeasonNumber == key.Item2);
            if (synopse == null)
            {
                return collection.FindAndRemove(query, SortBy.Null).Ok;
            }

            tvshow.SeasonSynopses.Remove(synopse);

            // In this case first remove the object synopse than remove the season, because can not have
            // one synopse for one season that not exists.
            return this.tvshowsRepository.Update(tvshow) && collection.FindAndRemove(query, SortBy.Null).Ok;
        }

        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        public void CreateAll(IEnumerable<Season> seasons)
        {
            var enumerable = seasons as List<Season> ?? seasons.ToList();
            if (!enumerable.Any())
            {
                return;
            }

            foreach (var season in enumerable)
            {
                this.Create(season);
            }
        }

        /// <summary>
        /// Get all seasons synopsis from one television show.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Season.SeasonSynopsis> GetAllFromOneTvShow(string tvshowId)
        {
            try
            {
                var tvshow = this.tvshowsRepository.Read(tvshowId);
                return tvshow.SeasonSynopses;
            }
            catch (Exception)
            {
                // TODO, add to log mechanism.
                return null;
            }
        }
    }
}
