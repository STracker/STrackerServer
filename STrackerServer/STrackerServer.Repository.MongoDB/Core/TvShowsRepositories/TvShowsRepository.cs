// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsRepository interface.
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;
    using STrackerServer.ImageConverter.Core;
    using STrackerServer.Logger.Core;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
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
        /// The collection of all television shows synopsis. In this case the 
        /// collection is always the same.
        /// </summary>
        private readonly MongoCollection collectionAll;

        /// <summary>
        /// The genres repository.
        /// </summary>
        private readonly IGenresRepository genresRepository;

        /// <summary>
        /// The comments repository.
        /// </summary>
        private readonly ITvShowCommentsRepository commentsRepository;

        /// <summary>
        /// The ratings repository.
        /// </summary>
        private readonly ITvShowRatingsRepository ratingsRepository;

        /// <summary>
        /// The new episodes repository.
        /// </summary>
        private readonly ITvShowNewEpisodesRepository newEpisodesRepository;

        /// <summary>
        /// The image converter.
        /// </summary>
        private readonly IImageConverter imageConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRepository"/> class.
        /// </summary>
        /// <param name="client">
        /// MongoDB client.
        /// </param>
        /// <param name="url">
        /// MongoDB url.
        /// </param>
        /// <param name="genresRepository">
        /// The genres Repository.
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
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="imageConverter">
        /// The image Converter.
        /// </param>
        public TvShowsRepository(MongoClient client, MongoUrl url, IGenresRepository genresRepository, ITvShowCommentsRepository commentsRepository, ITvShowRatingsRepository ratingsRepository, ITvShowNewEpisodesRepository newEpisodesRepository, ILogger logger, IImageConverter imageConverter)
            : base(client, url, logger)
        {
            this.genresRepository = genresRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
            this.newEpisodesRepository = newEpisodesRepository;
            this.imageConverter = imageConverter;

            this.collectionAll = this.Database.GetCollection(ConfigurationManager.AppSettings["AllTvShowsCollection"]);
        }

        /// <summary>
        /// Get one television show by is name.
        /// </summary>
        /// <param name="name">
        /// The name of the television show.
        /// </param>
        /// <param name="range">
        /// The range.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> ReadByName(string name, Range range)
        {
            var query = Query<TvShow.TvShowSynopsis>.Where(e => e.Name.ToLower().Contains(name.ToLower()));
            return this.collectionAll.FindAs<TvShow.TvShowSynopsis>(query)
                                    .SetSkip(range.Start)    
                                    .SetLimit(range.End)
                                    .ToList();
        }

        /// <summary>
        /// Add one season to television show's seasons synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSeason(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.Id, tvshowId);
            var update = Update<TvShow>.AddToSet(tv => tv.Seasons, season);
            return this.ModifyList(collection, query, update, this.Read(tvshowId));
        }

        /// <summary>
        /// Remove one season from television show's seasons synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSeason(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.Id, tvshowId);
            var update = Update<TvShow>.Pull(tv => tv.Seasons, season);
            return this.ModifyList(collection, query, update, this.Read(tvshowId));
        }

        /// <summary>
        /// Create one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(TvShow entity)
        {
            // Convert images.
            entity.Poster = this.imageConverter.Convert(entity.Poster, DefaultPoster);

            foreach (var actor in entity.Actors)
            {
                actor.Photo = this.imageConverter.Convert(actor.Photo, DefaultActorPhoto);
            }

            var collection = this.Database.GetCollection(entity.Id);

            // Setup the index in collection with all television shows synopsis.
            this.collectionAll.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // The order is relevant because mongo don't ensure transactions.
            collection.Insert(entity);
            this.collectionAll.Insert(entity.GetSynopsis());

            // Add the genres into collection of genres.
            foreach (var genre in entity.Genres)
            {
                this.genresRepository.AddTvShow(genre.Id, entity.GetSynopsis());
            }

            // Create the documents for comments, ratings and new episodes.
            this.commentsRepository.Create(new CommentsTvShow(entity.Id));
            this.ratingsRepository.Create(new RatingsTvShow(entity.Id));
            this.newEpisodesRepository.Create(new NewTvShowEpisodes(entity.Id) { TvShow = entity.GetSynopsis() });
        }

        /// <summary>
        /// Get one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/>.
        /// </returns>
        protected override TvShow HookRead(string id)
        {
            var collection = this.Database.GetCollection(id);
            return collection.FindOneByIdAs<TvShow>(id);
        }

        /// <summary>
        /// Update one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(TvShow entity)
        {
            var collection = this.Database.GetCollection(entity.Id);
            var oldEntity = this.Read(entity.Id);

            // Update the synopsis in genres.
            foreach (var genre in oldEntity.Genres)
            {
                this.genresRepository.RemoveTvShow(genre.Id, oldEntity.GetSynopsis());
                this.genresRepository.AddTvShow(genre.Id, entity.GetSynopsis());
            }

            // In new episodes document.
            var newEpis = this.newEpisodesRepository.Read(entity.Id);
            newEpis.TvShow = entity.GetSynopsis();
            this.newEpisodesRepository.Update(newEpis);

            // Finnaly, the document of the television show.
            var query = Query<TvShow>.EQ(tv => tv.Id, entity.Id);
            var update = Update<TvShow>.Replace(entity);

            collection.FindAndModify(query, SortBy.Null, update);
        }

        /// <summary>
        /// Delete one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// This method also deletes the information about seasons and episodes of
        /// television show.
        protected override void HookDelete(string id)
        {
            // Remove synopsis from genres.
            var tvshow = this.Read(id);
            foreach (var genre in tvshow.Genres)
            {
                this.genresRepository.RemoveTvShow(genre.Id, tvshow.GetSynopsis());
            }

            // Remove new episodes document.
            this.newEpisodesRepository.Delete(id);

            // Remove ratings.
            this.Database.DropCollection(string.Format("{0}-{1}", id, ConfigurationManager.AppSettings["RatingsCollection"]));

            // Remove comments.
            this.Database.DropCollection(string.Format("{0}-{1}", id, ConfigurationManager.AppSettings["CommentsCollection"]));

            // Remove the television show information.
            var query = Query<TvShow.TvShowSynopsis>.EQ(tv => tv.Id, id);
            this.collectionAll.FindAndRemove(query, SortBy.Null);
            this.Database.DropCollection(id); 
        }

        /// <summary>
        /// Hook method for Read all operation.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        protected override ICollection<TvShow> HookReadAll()
        {
            throw new NotSupportedException("this method currently is not supported.");
        }
    }
}