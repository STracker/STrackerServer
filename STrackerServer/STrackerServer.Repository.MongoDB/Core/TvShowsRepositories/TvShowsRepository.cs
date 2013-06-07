// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core.TvShowsRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Comments;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// Television shows repository for MongoDB database.
    /// </summary>
    public class TvShowsRepository : BaseRepository<TvShow, string>, ITvShowsRepository
    {
        /// <summary>
        /// The collection name for television show synopsis documents.
        /// </summary>
        private readonly string collectionNameForSynopsis;

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
        public TvShowsRepository(MongoClient client, MongoUrl url, IGenresRepository genresRepository, ITvShowCommentsRepository commentsRepository, ITvShowRatingsRepository ratingsRepository)
            : base(client, url)
        {
            this.genresRepository = genresRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;

            this.collectionNameForSynopsis = ConfigurationManager.AppSettings["AllTvShowsCollection"];
            this.collectionAll = this.Database.GetCollection(this.collectionNameForSynopsis);
        }

        /// <summary>
        /// The read all by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<TvShow.TvShowSynopsis> ReadAllByGenre(string genre)
        {
            return this.genresRepository.Read(genre).TvshowsSynopses;
        }

        /// <summary>
        /// The read by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.TvShow"/>.
        /// </returns>
        public TvShow ReadByName(string name)
        {
            try
            {
                var query = Query<TvShow.TvShowSynopsis>.EQ(e => e.Name, name);
                var synopse = this.collectionAll.FindOneAs<TvShow.TvShowSynopsis>(query);
                return synopse == null ? null : this.Read(synopse.Id);
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return null;
            }
        }

        /// <summary>
        /// The add season synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        public void AddSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, tvshowId);
            var update = Update<TvShow>.Push(tv => tv.SeasonSynopses, season);

            this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// The remove season synopsis.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="season">
        /// The season.
        /// </param>
        public void RemoveSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, tvshowId);
            var update = Update<TvShow>.Pull(tv => tv.SeasonSynopses, season);

            this.ModifyList(collection, query, update);
        }

        /// <summary>
        /// Create one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookCreate(TvShow entity)
        {
            var collection = this.Database.GetCollection(entity.TvShowId);

            // Setup the indexes.
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));
            this.collectionAll.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // The order is relevant because mongo don't ensure transactions.
            collection.Insert(entity);
            this.collectionAll.Insert(entity.GetSynopsis());

            // Add the genres into collection of genres.
            foreach (var genre in entity.Genres)
            {
                this.genresRepository.AddTvShowToGenre(entity.GetSynopsis(), genre);
            }

            // Create the documents for comments and ratings.
            this.commentsRepository.Create(new CommentsTvShow(entity.Key));
            this.ratingsRepository.Create(new RatingsTvShow(entity.Key));
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
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, id);

            return collection.FindOne<TvShow>(query, "_id", "Key");
        }

        /// <summary>
        /// Update one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        protected override void HookUpdate(TvShow entity)
        {
            throw new NotSupportedException("this method currently is not supported.");
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
            var tvshow = this.Read(id);
            foreach (var genre in tvshow.Genres)
            {
                this.genresRepository.RemoveTvShowFromGenre(tvshow.GetSynopsis(), genre);
            }

            var query = Query<TvShow.TvShowSynopsis>.EQ(tv => tv.Id, id);
            this.collectionAll.FindAndRemove(query, SortBy.Null);
            ((BaseCommentsRepository<CommentsTvShow, string>)this.commentsRepository).DropComments(id);
            ((BaseRatingsRepository<RatingsTvShow, string>)this.ratingsRepository).DropRatings(id);
            this.Database.DropCollection(id); 
        }
    }
}