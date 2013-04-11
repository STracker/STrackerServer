// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsFacade.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the TvShowsFacade type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerBusinessLayer.Facades
{
    using System;
    using System.Collections.Generic;

    using STrackerServerBusinessLayer.Core;

    using STrackerServerDatabase.Models;
    using STrackerServerDatabase.Repositories;

    /// <summary>
    /// The television show facade.
    /// </summary>
    public class TvShowsFacade : BaseCrudFacade<TvShow, string>, ITvShowsFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsFacade"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public TvShowsFacade(ITvShowsRepository repository)
            : base(repository)
        {
        }

        public override bool Create(TvShow entity)
        {
            throw new NotImplementedException();
        }

        public List<TvShow> ReadAllByGenre(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
