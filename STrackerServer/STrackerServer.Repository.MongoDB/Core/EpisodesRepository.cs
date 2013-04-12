// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRepository interface. This repository connects with MongoDB 
// database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System;
    using System.Linq;

    using global::MongoDB.Driver;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

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
        /// Initializes a new instance of the <see cref="EpisodesRepository"/> class.
        /// </summary>
        /// <param name="seasonsRepository">
        /// The seasons repository.
        /// </param>
        public EpisodesRepository(ISeasonsRepository seasonsRepository)
        {
            this.seasonsRepository = seasonsRepository;
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
        protected override bool Create(Episode entity, MongoCollection collection)
        {
            var season = seasonsRepository.Read(new Tuple<string, int>(entity.TvShowId, entity.SeasonNumber));

            if (season == null)
            {
                return false;
            }

            collection.Insert(entity);
            
            // Add the object synopsis to episodes synopses list of the season document.
            season.EpisodeSynopses.Add(entity.GetSynopsis());

            return seasonsRepository.Update(season);
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
        protected override bool Update(Episode entity, MongoCollection collection)
        {
            if (!collection.Save(entity).Ok)
            {
                return false;
            }
            
            var season = seasonsRepository.Read(new Tuple<string, int>(entity.TvShowId, entity.SeasonNumber));

            var synopse = season.EpisodeSynopses.FirstOrDefault(s => s.Number == entity.Number);

            season.EpisodeSynopses.Remove(synopse);

            synopse = entity.GetSynopsis();

            season.EpisodeSynopses.Add(synopse);

            return seasonsRepository.Update(season);
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
        protected override bool Delete(Tuple<string, int, int> id, MongoCollection collection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementation of get document collection hook method.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="MongoCollection"/>.
        /// </returns>
        protected override MongoCollection GetDocumentCollection(Tuple<string, int, int> id)
        {
            return Database.GetCollection(id.Item1);
        }
    }
}
