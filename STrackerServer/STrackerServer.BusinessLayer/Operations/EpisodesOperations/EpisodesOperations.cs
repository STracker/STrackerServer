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
    public class EpisodesOperations : BaseCrudOperations<IEpisodesRepository, Episode, Episode.EpisodeId>, IEpisodesOperations
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
        public override Episode Read(Episode.EpisodeId id)
        {
            var season = this.seasonsOperations.Read(new Season.SeasonId { TvShowId = id.TvShowId, SeasonNumber = id.SeasonNumber });
            return (season == null) ? null : this.Repository.Read(id);
        }

        /// <summary>
        /// Get all episodes from one season.
        /// </summary>
        /// <param name="id">
        /// The id of the season.
        /// </param>
        /// <returns>
        /// The <see cref="ICollection{T}"/>.
        /// </returns>
        public ICollection<Episode.EpisodeSynopsis> GetAllFromOneSeason(Season.SeasonId id)
        {
            var season = this.seasonsOperations.Read(new Season.SeasonId { TvShowId = id.TvShowId, SeasonNumber = id.SeasonNumber });
            return season == null ? null : season.Episodes;
        }
    }
}