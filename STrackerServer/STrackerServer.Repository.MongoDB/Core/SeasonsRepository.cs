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
            var collection = this.Database.GetCollection(tvshowId);

            var query = Query<Season>.EQ(s => s.TvShowId, tvshowId);

            var cursor = collection.FindAs<Season>(query);

            /*
             * Because television show document have also the TvShowId it will be returned. Needed to select all except the first document
             * wich is the television show document.
             */
            return cursor.Select(season => season.GetSynopsis()).ToList().Where(season => season.SeasonNumber > 0);
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
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Dont need to update the object synopse in television show seasons list because
        /// the synopse object holds only the season number and this number is never changed.
        public override bool Update(Season entity)
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

        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CreateAll(IEnumerable<Season> seasons)
        {
            var enumerable = seasons as List<Season> ?? seasons.ToList();
            if (!enumerable.Any())
            {
                return false;
            }

            var tvshowId = enumerable.ElementAt(0).TvShowId;

            var collection = this.Database.GetCollection(tvshowId);

            collection.InsertBatch(enumerable);

            // Add the synopsis to television show document.
            var tvshow = this.tvshowsRepository.Read(tvshowId);

            foreach (var season in enumerable)
            {
                tvshow.SeasonSynopses.Add(season.GetSynopsis());
            }

            return this.tvshowsRepository.Update(tvshow);
        }
    }
}
