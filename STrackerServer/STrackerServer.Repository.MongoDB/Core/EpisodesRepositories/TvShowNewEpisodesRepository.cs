// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowNewEpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRepository interface.
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// Television show new episodes repository
    /// </summary>
    public class TvShowNewEpisodesRepository : BaseRepository<NewTvShowEpisodes, string>, ITvShowNewEpisodesRepository 
    {
        /// <summary>
        /// The collection name.
        /// </summary>
        private readonly MongoCollection collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowNewEpisodesRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public TvShowNewEpisodesRepository(MongoClient client, MongoUrl url, ILogger logger)
            : base(client, url, logger)
        {
            this.collection = this.Database.GetCollection(ConfigurationManager.AppSettings["TvShowNewEpisodesCollection"]);
        }

        /// <summary>
        /// Add new episode. Only add if the date is not more old that the current date.
        /// </summary>
        /// <param name="id">
        /// The id of television show.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddEpisode(string id, Episode.EpisodeSynopsis episode)
        {
            DateTime temp;

            if (episode.Date != null && !DateTime.TryParse(episode.Date, out temp))
            {
                return false;
            }

            if (DateTime.Parse(episode.Date) < DateTime.UtcNow.Date)
            {
                return false;
            }

            var query = Query<NewTvShowEpisodes>.EQ(ne => ne.Id, id);
            var update = Update<NewTvShowEpisodes>.AddToSet(ne => ne.Episodes, episode);
            return this.ModifyList(this.collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Remove episode.
        /// </summary>
        /// <param name="id">
        /// The id of television show.
        /// </param>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveEpisode(string id, Episode.EpisodeSynopsis episode)
        {
            var query = Query<NewTvShowEpisodes>.EQ(ne => ne.Id, id);
            var update = Update<NewTvShowEpisodes>.Pull(ne => ne.Episodes, episode);
            return this.ModifyList(this.collection, query, update, this.Read(id));
        }

        /// <summary>
        /// Delete old episodes, i.e., the episodes with old dates.
        /// </summary>
        public void DeleteOldEpisodes()
        {
            var all = this.HookReadAll();
            foreach (var episodes in all)
            {
                var oldOnes = episodes.Episodes.Where(e => DateTime.Parse(e.Date) < DateTime.UtcNow.Date);
                foreach (var oldOne in oldOnes)
                {
                    var query = Query<NewTvShowEpisodes>.EQ(e => e.Id, episodes.Id);
                    var update = Update<NewTvShowEpisodes>.Pull(e => e.Episodes, oldOne);
                    this.ModifyList(this.collection, query, update, episodes);
                }
            }
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(NewTvShowEpisodes entity)
        {
            this.collection.Insert(entity);
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="NewTvShowEpisodes"/>.
        /// </returns>
        protected override NewTvShowEpisodes HookRead(string id)
        {
            return this.collection.FindOneByIdAs<NewTvShowEpisodes>(id);
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(NewTvShowEpisodes entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(string id)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<NewTvShowEpisodes> HookReadAll()
        {
            return this.collection.FindAllAs<NewTvShowEpisodes>().ToList();
        }
    }
}
