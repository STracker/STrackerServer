// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowRepository.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repositories
{
    using STrackerServer.Models.Domain;

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
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(TvShow entity)
        {
            // TODO
            return false;
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
            // TODO
            return null;
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