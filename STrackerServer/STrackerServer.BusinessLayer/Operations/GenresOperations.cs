// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenresOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The genre operations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The genre operations.
    /// </summary>
    public class GenresOperations : BaseCrudOperations<IGenresRepository, Genre, string>, IGenresOperations
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
        /// Get all synopsis from all genres.
        /// </summary>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<Genre.GenreSynopsis> ReadAllSynopsis()
        {
            return this.Repository.ReadAllSynopsis();
        }
    }
}
