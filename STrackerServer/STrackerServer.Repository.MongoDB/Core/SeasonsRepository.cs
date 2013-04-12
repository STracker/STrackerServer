// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ISeasonsRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;
    using System.Linq;

    using global::MongoDB.Bson.Serialization;

    using global::MongoDB.Driver;

    using global::MongoDB.Driver.Builders;

    using STrackerServer.DataAccessLayer.Core;
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
        /// Initializes static members of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        static SeasonsRepository()
        {
            BsonClassMap.RegisterClassMap<Season>(
                cm => cm.MapIdField(c => c.Id.ToString()));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsRepository"/> class.
        /// </summary>
        /// <param name="tvshowsRepository">
        /// Television shows repository.
        /// </param>
        public SeasonsRepository(ITvShowsRepository tvshowsRepository)
        {
            this.tvshowsRepository = tvshowsRepository;
        }

        /// <summary>
        /// Implementation of Create hook method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool Create(Season entity, MongoCollection collection)
        {
            var tvshow = this.tvshowsRepository.Read(entity.TvShowId);

            if (tvshow == null)
            {
                return false;
            }

            collection.Insert(entity);

            // Add the object synopsis to seasons synopses list of the television show document.
            tvshow.SeasonSynopses.Add(entity.GetSynopsis());

            return this.tvshowsRepository.Update(tvshow);
        }

        /// <summary>
        /// Implementation of Update hook method.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Dont needed to update the synopse object in television show document
        /// because the synopse object only have the season number and his value
        /// is always the same.
        protected override bool Update(Season entity, MongoCollection collection)
        {
            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// Implementation of Delete hook method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// Need to delete the season synopse of television show document before
        /// removing the season document.
        protected override bool Delete(Tuple<string, int> id, MongoCollection collection)
        {
            var tvshow = this.tvshowsRepository.Read(id.Item1);

            var seasonsynopse = tvshow.SeasonSynopses.FirstOrDefault(season => season.Number == id.Item2);

            tvshow.SeasonSynopses.Remove(seasonsynopse);

            return this.tvshowsRepository.Update(tvshow) && collection.FindAndRemove(Query.EQ("_id", id.ToString()), SortBy.Null).Ok;
        }

        /// <summary>
        /// The get document collection.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="MongoCollection"/>.
        /// </returns>
        protected override MongoCollection GetDocumentCollection(Tuple<string, int> id)
        {
            return this.Database.GetCollection(id.Item1);
        }
    }
}
