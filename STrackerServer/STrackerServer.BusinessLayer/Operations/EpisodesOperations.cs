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
    using System.Collections.Generic;

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
        /// Get one episode from one season from one television show.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        public override Episode Read(Tuple<string, int, int> id)
        {
            var season = this.seasonsOperations.Read(new Tuple<string, int>(id.Item1, id.Item2));
            return (season == null) ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="tvshowId">
        /// The television show id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode.EpisodeSynopsis> GetAllFromOneSeason(string tvshowId, int seasonNumber)
        {
            var season = this.seasonsOperations.Read(new Tuple<string, int>(tvshowId, seasonNumber));
            return season == null ? null : ((IEpisodesRepository)this.Repository).GetAllFromOneSeason(tvshowId, seasonNumber);
        }
    }
}