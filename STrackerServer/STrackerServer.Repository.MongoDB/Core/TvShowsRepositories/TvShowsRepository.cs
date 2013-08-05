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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
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
        /// The newest episodes repository.
        /// </summary>
        private readonly INewestEpisodesRepository newestEpisodesRepository;

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
        /// <param name="newestEpisodesRepository">
        /// The newest Episodes Repository.
        /// </param>
        public TvShowsRepository(MongoClient client, MongoUrl url, IGenresRepository genresRepository, ITvShowCommentsRepository commentsRepository, ITvShowRatingsRepository ratingsRepository, INewestEpisodesRepository newestEpisodesRepository)
            : base(client, url)
        {
            this.genresRepository = genresRepository;
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
            this.newestEpisodesRepository = newestEpisodesRepository;

            this.collectionAll = this.Database.GetCollection(ConfigurationManager.AppSettings["AllTvShowsCollection"]);
        }

        /// <summary>
        /// The read by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public ICollection<TvShow.TvShowSynopsis> ReadByName(string name)
        {
            var query = Query<TvShow.TvShowSynopsis>.Where(e => e.Name.ToUpper().Contains(name.ToUpper()));
            return this.collectionAll.FindAs<TvShow.TvShowSynopsis>(query).ToList();
        }

        /// <summary>
        /// The get names.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public string[] GetNames(string query)
        {
            return this.collectionAll.FindAllAs<TvShow.TvShowSynopsis>().Select(synopsis => synopsis.Name).Where(s => s.ToUpper().Contains(query.ToUpper())).ToArray();
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
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.Id, tvshowId);
            var update = Update<TvShow>.Push(tv => tv.SeasonSynopsis, season);

            return this.ModifyList(collection, query, update, this.Read(tvshowId));
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
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool RemoveSeasonSynopsis(string tvshowId, Season.SeasonSynopsis season)
        {
            var collection = this.Database.GetCollection(tvshowId);
            var query = Query<TvShow>.EQ(tv => tv.Id, tvshowId);
            var update = Update<TvShow>.Pull(tv => tv.SeasonSynopsis, season);

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
            var collection = this.Database.GetCollection(entity.Id);

            // Setup the index for name in collection that have all tv shows synopsis.
            this.collectionAll.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // The order is relevant because mongo don't ensure transactions.
            collection.Insert(entity);
            this.collectionAll.Insert(entity.GetSynopsis());

            // Add the genres into collection of genres.
            foreach (var genre in entity.Genres)
            {
                this.genresRepository.AddTvShowToGenre(genre.Id, entity.GetSynopsis());
            }

            // Create the documents for comments and ratings.
            this.commentsRepository.Create(new CommentsTvShow(entity.Id));
            this.ratingsRepository.Create(new RatingsTvShow(entity.Id));
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
            // First change the synopsis.
            // Find to remove then insert the synopsis object in all collection.
            var query = Query<TvShow.TvShowSynopsis>.EQ(tv => tv.Id, entity.Id);
            this.collectionAll.FindAndRemove(query, SortBy.Null);
            this.collectionAll.Insert(entity.GetSynopsis());

            var collection = this.Database.GetCollection(entity.Id);

            var update = Update<TvShow>.Set(tv => tv.Name, entity.Name)
                .Set(tv => tv.Description, entity.Description)
                .Set(tv => tv.AirDay, entity.AirDay)
                .Set(tv => tv.AirTime, entity.AirTime)
                .Set(tv => tv.FirstAired, entity.FirstAired)
                .Set(tv => tv.Runtime, entity.Runtime)
                .Set(tv => tv.Poster, entity.Poster)
                .Set(tv => tv.Actors, entity.Actors)
                .Set(tv => tv.Version, entity.Version + 1);

            query = Query<TvShow>.EQ(tv => tv.Id, entity.Id);

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
            // Remove television show information from genres.
            var tvshow = this.Read(id);
            foreach (var genre in tvshow.Genres)
            {
                this.genresRepository.RemoveTvShowFromGenre(genre.Id, tvshow.GetSynopsis());
            }

            // Remove from collection that contains all.
            var query = Query<TvShow.TvShowSynopsis>.EQ(tv => tv.Id, id);
            this.collectionAll.FindAndRemove(query, SortBy.Null);

            // Remove comments and ratings.
            this.commentsRepository.RemoveAllComments(id);
            this.ratingsRepository.RemoveAllRatings(id);

            // Remove the newest episodes document from this tvshow.
            this.newestEpisodesRepository.Delete(id);

            // Drop the collection.
            this.Database.DropCollection(id); 
        }
    }
}