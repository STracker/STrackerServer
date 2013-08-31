// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.SeasonsRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// Seasons repository for MongoDB database.
    /// </summary>
    public class SeasonsRepository : BaseRepository<Season, Season.SeasonId>, ISeasonsRepository
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
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SeasonsRepository(ITvShowsRepository tvshowsRepository, MongoClient client, MongoUrl url, ILogger logger) 
            : base(client, url, logger)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        /// <summary>
        /// Create several seasons.
        /// </summary>
        /// <param name="seasons">
        /// The seasons.
        /// </param>
        public void CreateAll(ICollection<Season> seasons)
        {
            foreach (var season in seasons)
            {
                this.Create(season);
            }
        }

        /// <summary>
        /// Add one episode synopsis to season's episodes list.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddEpisode(Season.SeasonId id, Episode.EpisodeSynopsis episode)
        {
            var collection = this.Database.GetCollection(id.TvShowId);
            var query = Query<Season>.EQ(s => s.Id, id);
            var update = Update<Season>.AddToSet(s => s.Episodes, episode);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Remove one episode synopsis from season's episodes list.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveEpisode(Season.SeasonId id, Episode.EpisodeSynopsis episode)
        {
            var collection = this.Database.GetCollection(id.TvShowId);
            var query = Query<Season>.EQ(s => s.Id, id);
            var update = Update<Season>.Pull(s => s.Episodes, episode);
            return this.ModifyList(collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Create one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(Season entity)
        {
            var collection = this.Database.GetCollection(entity.Id.TvShowId);
            collection.Insert(entity);

            // Add the synospis object to television show document.
            this.tvshowsRepository.AddSeason(entity.Id.TvShowId, entity.GetSynopsis());
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        protected override Season HookRead(Season.SeasonId id)
        {
            var collection = this.Database.GetCollection(id.TvShowId);

            var season = collection.FindOneByIdAs<Season>(id.ToBsonDocument());
            season.Episodes = season.Episodes.OrderBy(synopsis => synopsis.Id.EpisodeNumber).ToList();

            return season;
        }

        /// <summary>
        /// Update one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(Season entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Season.SeasonId id)
        {
            var collection = this.Database.GetCollection(id.TvShowId);
            var query = Query<Season>.EQ(s => s.Id, id);
            var synopsis = this.Read(id).GetSynopsis();

            // In this case first remove the object synopse than remove the season, because can not have
            // one synopse for one season that not exists.
            this.tvshowsRepository.RemoveSeason(id.TvShowId, synopsis);
            collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<Season> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}