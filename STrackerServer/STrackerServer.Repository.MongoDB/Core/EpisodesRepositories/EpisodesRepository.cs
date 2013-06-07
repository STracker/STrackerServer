// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.EpisodesRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

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
        /// The comments repository.
        /// </summary>
        private readonly IEpisodeCommentsRepository commentsRepository;

        /// <summary>
        /// The ratings repository.
        /// </summary>
        private readonly IEpisodeRatingsRepository ratingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        /// <param name="commentsRepository">
        /// The comments Repository.
        /// </param>
        /// <param name="ratingsRepository">
        /// The ratings Repository.
        /// </param>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository, IEpisodeCommentsRepository commentsRepository, IEpisodeRatingsRepository ratingsRepository, MongoClient client, MongoUrl url) 
            : base(client, url)
        {
            this.seasonsRepository = seasonsRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
        }

        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        public void CreateAll(IEnumerable<Episode> episodes)
        {
            var enumerable = episodes as List<Episode> ?? episodes.ToList();
            if (!enumerable.Any())
            {
                return;
            }

            foreach (var episode in enumerable)
            {
                this.Create(episode);
            }
        }

        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode.EpisodeSynopsis> GetAllFromOneSeason(string tvshowId, int seasonNumber)
        {
            return this.seasonsRepository.Read(new Tuple<string, int>(tvshowId, seasonNumber)).EpisodeSynopses;
        }

        /// <summary>
        /// Create one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Needs to create also the object synopse in season episodes list.
        protected override void HookCreate(Episode entity)
        {
            var collection = this.Database.GetCollection(entity.TvShowId);
            collection.Insert(entity);

            // Add the synopse of the entity to season.
            this.seasonsRepository.AddEpisodeSynopsis(entity.TvShowId, entity.SeasonNumber, entity.GetSynopsis());

            // Also create the documents for comments and ratings.
            this.commentsRepository.Create(new CommentsEpisode(entity.Key));
            this.ratingsRepository.Create(new RatingsEpisode(entity.Key));
        }

        /// <summary>
        /// Get one episode.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.Episode"/>.
        /// </returns>
        protected override Episode HookRead(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, id.Item1), Query<Episode>.EQ(e => e.SeasonNumber, id.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, id.Item3));
            return collection.FindOne<Episode>(query, "_id", "Key");
        }

        /// <summary>
        /// Update one episode.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// Needs to update also the object synopse in season episodes list.
        protected override void HookUpdate(Episode entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
        }

        /// <summary>
        /// Delete one episode.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// Needs to delete also the object synopse in season episodes list.
        protected override void HookDelete(Tuple<string, int, int> id)
        {
            var collection = this.Database.GetCollection(id.Item1);
            var query = Query.And(Query<Episode>.EQ(e => e.TvShowId, id.Item1), Query<Episode>.EQ(e => e.SeasonNumber, id.Item2), Query<Episode>.EQ(e => e.EpisodeNumber, id.Item3));

            var synopsis = this.Read(id).GetSynopsis();

            // In this case first remove the object synopse than remove the episode, because can not have
            // one synopse for one episode that not exists.
            this.seasonsRepository.RemoveEpisodeSynopsis(id.Item1, id.Item2, synopsis);
            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}