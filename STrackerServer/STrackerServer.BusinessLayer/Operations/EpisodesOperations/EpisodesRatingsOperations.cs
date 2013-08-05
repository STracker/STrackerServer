// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of IEpisodesRatingsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.EpisodesOperations
{
    using STrackerServer.BusinessLayer.Core.EpisodesOperations;
    using STrackerServer.DataAccessLayer.Core.EpisodesRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The episodes ratings operations.
    /// </summary>
    public class EpisodesRatingsOperations : BaseRatingsOperations<Episode, RatingsEpisode, Episode.EpisodeKey>, IEpisodesRatingsOperations 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesRatingsOperations"/> class.
        /// </summary>
        /// <param name="ratingsRepository">
        /// The ratings repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public EpisodesRatingsOperations(IEpisodeRatingsRepository ratingsRepository, IEpisodesRepository repository)
            : base(ratingsRepository, repository)
        {
        }
    }
}