// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomTvShowsRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of custom television shows repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.CustomRepositories
{
    using System;
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The custom television shows repository.
    /// </summary>
    public class CustomTvShowsRepository : ITvShowsRepository
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IDictionary<string, TvShow> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTvShowsRepository"/> class.
        /// </summary>
        public CustomTvShowsRepository()
        {
            this.repository = new Dictionary<string, TvShow>();
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Create(TvShow entity)
        {
            this.repository.Add(entity.Key, entity);
            return true;
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow Read(string key)
        {
            return this.repository[key];
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Update(TvShow entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Delete(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The get all by genre.
        /// </summary>
        /// <param name="genre">
        /// The genre.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>List</cref>
        ///     </see> .
        /// </returns>
        public List<TvShow> GetAllByGenre(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
