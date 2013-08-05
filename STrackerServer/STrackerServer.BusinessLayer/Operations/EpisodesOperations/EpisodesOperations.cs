// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using System.Collections.Generic;

    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.BusinessLayer.Core.SeasonsOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// Episodes operations.
    /// </summary>
    public class EpisodesOperations : BaseCrudOperations<Episode, Episode.EpisodeKey>, IEpisodesOperations
    {
        /// <summary>
        /// The seasons operations.
        /// </summary>
        private readonly ISeasonsOperations seasonsOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesOperations"/> class.
        /// </summary>
        /// <param name="seasonsOperations">
        /// The seasons operations.
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
        /// The <see cref="STrackerServer.DataAccessLayer.DomainEntities.Episode"/>.
        /// </returns>
        public override Episode Read(Episode.EpisodeKey id)
        {
            var season = this.seasonsOperations.Read(new Season.SeasonKey { TvshowId = id.TvshowId, SeasonNumber = id.SeasonNumber });
            return (season == null) ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// The get all from one season.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public ICollection<Episode.EpisodeSynopsis> GetAllFromOneSeason(Season.SeasonKey id)
        {
            var season = this.seasonsOperations.Read(id);
            return season == null ? null : ((IEpisodesRepository)this.Repository).GetAllFromOneSeason(id);
        }
    }
}