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
    using System.Collections.Generic;
    using System.Linq;

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// Episodes repository for MongoDB database.
    /// </summary>
    public class EpisodesRepository : BaseRepository<Episode, Episode.EpisodeKey>, IEpisodesRepository
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
        /// The newest episodes repository.
        /// </summary>
        private readonly INewestEpisodesRepository newestEpisodesRepository;

        /// <summary>
        /// The television shows repository.
        /// </summary>
        private readonly ITvShowsRepository tvshowsRepository;

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
        /// <param name="newestEpisodesRepository">
        /// The newest Episodes Repository.
        /// </param>
        /// <param name="tvshowsRepository">
        /// The television shows Repository.
        /// </param>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository, IEpisodeCommentsRepository commentsRepository, IEpisodeRatingsRepository ratingsRepository, INewestEpisodesRepository newestEpisodesRepository, ITvShowsRepository tvshowsRepository, MongoClient client, MongoUrl url) 
            : base(client, url)
        {
            this.seasonsRepository = seasonsRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
            this.newestEpisodesRepository = newestEpisodesRepository;
            this.tvshowsRepository = tvshowsRepository;
        }

        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        public void CreateAll(ICollection<Episode> episodes)
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
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Episode.EpisodeSynopsis> GetAllFromOneSeason(Season.SeasonKey id)
        {
            return this.seasonsRepository.Read(id).EpisodeSynopsis;
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
            var collection = this.Database.GetCollection(entity.Id.TvshowId);
            collection.Insert(entity);

            // Add the synopse of the entity to season.
            this.seasonsRepository.AddEpisodeSynopsis(new Season.SeasonKey { TvshowId = entity.Id.TvshowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis());

            // Try add to newest document.
            this.newestEpisodesRepository.TryAddNewestEpisode(entity.GetSynopsis(), this.tvshowsRepository.Read(entity.Id.TvshowId).GetSynopsis());

            // Also create the documents for comments and ratings.
            this.commentsRepository.Create(new CommentsEpisode(entity.Id));
            this.ratingsRepository.Create(new RatingsEpisode(entity.Id));
        }

        /// <summary>
        /// Hook method for Read operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        protected override Episode HookRead(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(id.TvshowId);
            return collection.FindOneByIdAs<Episode>(id.ToBsonDocument());
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
            // Remove the synopsis from season and insert the new one.
            if (!(this.seasonsRepository.RemoveEpisodeSynopsis(new Season.SeasonKey { TvshowId = entity.Id.TvshowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis()) &&
                this.seasonsRepository.AddEpisodeSynopsis(new Season.SeasonKey { TvshowId = entity.Id.TvshowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis())))
            {
                return;
            }

            // Try add to newest document. This episode can be one new episode with new date.
            this.newestEpisodesRepository.TryAddNewestEpisode(entity.GetSynopsis(), this.tvshowsRepository.Read(entity.Id.TvshowId).GetSynopsis());

            var collection = this.Database.GetCollection(entity.Id.TvshowId);
            var query = Query<Episode>.EQ(e => e.Id, entity.Id);
            var update = Update<Episode>.Set(e => e.Name, entity.Name)
                .Set(e => e.Description, entity.Description)
                .Set(e => e.Date, entity.Date)
                .Set(e => e.Poster, entity.Poster)
                .Set(e => e.GuestActors, entity.GuestActors)
                .Set(e => e.Directors, entity.Directors)
                .Set(e => e.Version, entity.Version + 1);

            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Episode.EpisodeKey id)
        {
            var collection = this.Database.GetCollection(id.TvshowId);
            var query = Query<Episode>.EQ(e => e.Id, id);

            var synopsis = this.Read(id).GetSynopsis();

            // In this case first remove the object synopse than remove the episode, because can not have
            // one synopse for one episode that not exists.
            this.seasonsRepository.RemoveEpisodeSynopsis(new Season.SeasonKey { TvshowId = id.TvshowId, SeasonNumber = id.SeasonNumber }, synopsis);

            // Remove from newest episodes.
            this.newestEpisodesRepository.RemoveEpisode(synopsis);

            collection.FindAndRemove(query, SortBy.Null);
        }
    }
}