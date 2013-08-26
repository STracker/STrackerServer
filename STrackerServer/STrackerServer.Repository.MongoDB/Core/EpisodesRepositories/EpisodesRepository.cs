// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepository.cs" company="STracker">
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

    using global::MongoDB.Bson;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.SeasonsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;
    using STrackerServer.ImageConverter.Core;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// Episodes repository for MongoDB database.
    /// </summary>
    public class EpisodesRepository : BaseRepository<Episode, Episode.EpisodeId>, IEpisodesRepository
    {
        /// <summary>
        /// The default actor photo.
        /// </summary>
        private const string DefaultActorPhoto = "https://dl.dropboxusercontent.com/u/2696848/default-profile-pic.jpg";

        /// <summary>
        /// The default poster.
        /// </summary>
        private const string DefaultPoster = "https://dl.dropboxusercontent.com/u/2696848/image-not-found.gif";
        
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
        /// The new episodes repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository newEpisodesRepository;

        /// <summary>
        /// The image converter.
        /// </summary>
        private readonly IImageConverter imageConverter;

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
        /// <param name="newEpisodesRepository">
        /// The new Episodes Repository.
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
        /// <param name="imageConverter">
        /// The image Converter.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository, IEpisodeCommentsRepository commentsRepository, IEpisodeRatingsRepository ratingsRepository, ITvShowNewEpisodesRepository newEpisodesRepository, MongoClient client, MongoUrl url, ILogger logger, IImageConverter imageConverter) 
            : base(client, url, logger)
        {
            this.seasonsRepository = seasonsRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
            this.newEpisodesRepository = newEpisodesRepository;
            this.imageConverter = imageConverter;
        }

        /// <summary>
        /// Create several episodes.
        /// </summary>
        /// <param name="episodes">
        /// The episodes.
        /// </param>
        public void CreateAll(ICollection<Episode> episodes)
        {
            foreach (var episode in episodes)
            {
                this.Create(episode);
            }
        }

        /// <summary>
        /// Hook method for Create operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(Episode entity)
        {
            // Convert Images
            entity.Poster = this.imageConverter.Convert(entity.Poster, DefaultPoster);

            foreach (var guestActor in entity.GuestActors)
            {
                guestActor.Photo = this.imageConverter.Convert(guestActor.Photo, DefaultActorPhoto);
            }

            foreach (var director in entity.Directors)
            {
                director.Photo = this.imageConverter.Convert(director.Photo, DefaultActorPhoto);
            }

            var collection = this.Database.GetCollection(entity.Id.TvShowId);
            collection.Insert(entity);

            // Add the synopse of the entity to season.
            this.seasonsRepository.AddEpisode(new Season.SeasonId { TvShowId = entity.Id.TvShowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis());

            // Try add to newest document.
            this.newEpisodesRepository.AddEpisode(entity.Id.TvShowId, entity.GetSynopsis());

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
        protected override Episode HookRead(Episode.EpisodeId id)
        {
            var collection = this.Database.GetCollection(id.TvShowId);
            return collection.FindOneByIdAs<Episode>(id.ToBsonDocument());
        }

        /// <summary>
        /// Hook method for Update operation.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(Episode entity)
        {
            // Update the synopsis in new episodes.
            this.newEpisodesRepository.RemoveEpisode(entity.Id.TvShowId, entity.GetSynopsis());
            this.newEpisodesRepository.AddEpisode(entity.Id.TvShowId, entity.GetSynopsis());

            // Update the synopsis in season.
            this.seasonsRepository.RemoveEpisode(new Season.SeasonId { TvShowId = entity.Id.TvShowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis());
            this.seasonsRepository.AddEpisode(new Season.SeasonId { TvShowId = entity.Id.TvShowId, SeasonNumber = entity.Id.SeasonNumber }, entity.GetSynopsis());

            // Update the document of the episode.
            var collection = this.Database.GetCollection(entity.Id.TvShowId);
            var query = Query<Episode>.EQ(e => e.Id, entity.Id);
            var update = Update<Episode>.Replace(entity);
            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Hook method for Delete operation.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected override void HookDelete(Episode.EpisodeId id)
        {
            var episode = this.Read(id).GetSynopsis();

            // Remove the synopsis in new episodes.
            this.newEpisodesRepository.RemoveEpisode(id.TvShowId, episode);

            // Remove the synpopsis in season.
            this.seasonsRepository.RemoveEpisode(new Season.SeasonId { TvShowId = id.TvShowId, SeasonNumber = id.SeasonNumber }, episode);

            // Remove the document of episode.
            var collection = this.Database.GetCollection(id.TvShowId);
            var query = Query<Episode>.EQ(e => e.Id, id);
            collection.FindAndRemove(query, SortBy.Null);
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<Episode> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}