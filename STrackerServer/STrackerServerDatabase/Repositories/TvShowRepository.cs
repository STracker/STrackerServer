// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using MongoDB.Driver.Builders;

    using STrackerServerDatabase.Models;

    /// <summary>
    /// Repository of television shows.
    /// </summary>
    public class TvShowRepository : Repository<string, TvShow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowRepository"/> class.
        /// </summary>
        public TvShowRepository()
            : base("tvShows")
        {
        }

        /// <summary>
        /// Create a new television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(TvShow entity)
        {
            return Collection.Insert(entity).Ok;
        }

        /// <summary>
        /// Get a television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public override TvShow Read(string id)
        {
            var query = Query<TvShow>.EQ(tv => tv.Id, id);

            return Collection.FindOneAs<TvShow>(query);
        }

        /// <summary>
        /// Update the desire television show.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(TvShow entity)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Delete a television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(string id)
        {
            // TODO
            return false;
        }
    }
}