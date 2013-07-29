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

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
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
            return this.tvshowsRepository.Read(tvshowId).SeasonSynopsis;
        }

        /// <summary>
        /// The add episode synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        public void AddEpisodeSynopsis(string tvshowId, int seasonNumber, Episode.EpisodeSynopsis episode)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, tvshowId), Query<Season>.EQ(s => s.SeasonNumber, seasonNumber));
            var update = Update<Season>.Push(s => s.EpisodeSynopsis, episode);

            this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// The remove episode synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// Television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        public void RemoveEpisodeSynopsis(string tvshowId, int seasonNumber, Episode.EpisodeSynopsis episode)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, tvshowId), Query<Season>.EQ(s => s.SeasonNumber, seasonNumber));
            var update = Update<Season>.Pull(s => s.EpisodeSynopsis, episode);

            this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Create one season.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Needs to create also the object synopse in television show seasons list.
        protected override void HookCreate(Season entity)
        {
            var collection = this.Database.GetCollection(entity.TvShowId);
            collection.Insert(entity);

            // Add the synospis object to television show document.
            this.tvshowsRepository.AddSeasonSynopsis(entity.TvShowId, entity.GetSynopsis());
        }

        /// <summary>
        /// Get one season.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        protected override Season HookRead(Tuple<string, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, id.Item1), Query<Season>.EQ(s => s.SeasonNumber, id.Item2));
            return collection.FindOne<Season>(query, "_id");
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
        /// Delete one season.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// Needs to delete also the object synopse in television show seasons list.
        protected override void HookDelete(Tuple<string, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Season>.EQ(s => s.TvShowId, id.Item1), Query<Season>.EQ(s => s.SeasonNumber, id.Item2));

            var synopsis = this.Read(id).GetSynopsis();

            // In this case first remove the object synopse than remove the season, because can not have
            // one synopse for one season that not exists.
            this.tvshowsRepository.RemoveSeasonSynopsis(id.Item1, synopsis);
            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}