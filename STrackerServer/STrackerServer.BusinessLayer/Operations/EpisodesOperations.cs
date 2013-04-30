// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations
{
    using System;

    using STrackerServer.BusinessLayer.Core;
    using STrackerServer.DataAccessLayer.Core;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations.
    /// </summary>
    public class EpisodesOperations : BaseCrudOperations<Episode, Tuple<string, int, int>>, IEpisodesOperations
    {
        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperations"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The seasons Operations.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public EpisodesOperations(ISeasonsOperations seasonsOperations, IEpisodesRepository repository)
            : base(repository)
        {
            this.seasonsOperations = seasonsOperations;
        }

        /// <summary>
        /// Try get one episode.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        public Episode TryRead(Tuple<string, int, int> id, out OperationResultState state)
        {
            var season = this.seasonsOperations.TryRead(new Tuple<string, int>(id.Item1, id.Item2), out state);
            if (state == OperationResultState.Completed && season != null)
            {
                return this.Repository.Read(id);
            }

            return null;
        }
    }
}