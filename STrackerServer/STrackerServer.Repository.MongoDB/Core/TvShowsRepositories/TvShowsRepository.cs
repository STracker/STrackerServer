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
        private const string CollectionNameForSynopsis = "Tvshows";

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
        }

        /// <summary>
        /// Create one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void HookCreate(TvShow entity)
        {
            var collection = this.Database.GetCollection(entity.TvShowId);

            // Get the collection that have all references for all television shows.
            var collectionAll = this.Database.GetCollection(CollectionNameForSynopsis);

            // Setup the ensure indexes.
            collection.EnsureIndex(new IndexKeysBuilder().Ascending("TvShowId", "SeasonNumber", "EpisodeNumber"), IndexOptions.SetUnique(true));
            collectionAll.EnsureIndex(new IndexKeysBuilder().Ascending("Name"));

            // The order is relevant because mongo don't ensure transactions.
            collection.Insert(entity);
            collectionAll.Insert(entity.GetSynopsis());

            // Add the genres into collection of genres.
            foreach (var genre in entity.Genres)
            {
                this.genresRepository.AddTvShowToGenre(entity.GetSynopsis(), genre);
            }

            // Create the document for comments.
            this.commentsRepository.Create(new CommentsTvShow { TvShowId = entity.TvShowId });
            this.ratingsRepository.Create(new RatingsTvShow { TvShowId = entity.TvShowId });
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
        public override TvShow HookRead(string id)
        {
            var collection = this.Database.GetCollection(id);
            var query = Query<TvShow>.EQ(tv => tv.TvShowId, id);

            var tvshow = collection.FindOneAs<TvShow>(query);
            if (tvshow == null)
            {
                return null;
            }

            tvshow.Id = id;
            return tvshow;
        }

        /// <summary>
        /// Update one television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void HookUpdate(TvShow entity)
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
        public override void HookDelete(string id)
        {
            var tvshow = this.Read(id);
            if (tvshow == null)
            {
                return;
            }

            foreach (var genre in tvshow.Genres)
            {
                this.genresRepository.RemoveTvShowFromGenre(tvshow.GetSynopsis(), genre);
            }

            var collectionAll = this.Database.GetCollection(CollectionNameForSynopsis);
            var query = Query<TvShow.TvShowSynopsis>.EQ(tv => tv.Id, id);
           
            this.Database.DropCollection(id); 
            collectionAll.FindAndRemove(query, SortBy.Null);
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
            var collection = this.Database.GetCollection<TvShow.TvShowSynopsis>(CollectionNameForSynopsis);
            var query = Query<TvShow.TvShowSynopsis>.EQ(e => e.Name, name);

            try
            {
                var synopse = collection.FindOneAs<TvShow.TvShowSynopsis>(query);
                if (synopse == null)
                {
                    return null;
                }

                var tvshowCollection = this.Database.GetCollection(synopse.Id);
                query = Query<TvShow>.EQ(tv => tv.TvShowId, synopse.Id);
                return tvshowCollection.FindOneAs<TvShow>(query);
            }
            catch (Exception)
            {
                // TODO, add exception to Log mechanism.
                return null;
            }
        }
    }
}