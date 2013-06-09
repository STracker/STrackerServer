// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genre operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.TvShowsOperations
{
    using System.Collections.Generic;
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The genre operations.
    /// </summary>
    public class GenresOperations : BaseCrudOperations<Genre, string>, IGenresOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenresOperations"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public GenresOperations(IGenresRepository repository)
            : base(repository)
        {
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Genre"/>.
        /// </returns>
        public override Genre Read(string id)
        {
            return id == null ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public List<Genre.GenreSynopsis> GetAll()
        {
            var list = ((IGenresRepository)Repository).GetAll();
            return list.Select(genre => genre.GetSynopsis()).ToList();
        }
    }
}
