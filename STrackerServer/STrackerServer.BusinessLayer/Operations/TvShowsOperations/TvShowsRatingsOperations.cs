// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowsRatingsOperations.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of ITvShowsRatingsOperations interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Operations.TvShowsOperations
{
    using System.Linq;

    using STrackerServer.BusinessLayer.Core.TvShowsOperations;
    using STrackerServer.DataAccessLayer.Core.TvShowsRepositories;
    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.Ratings;

    /// <summary>
    /// The television shows ratings operations.
    /// </summary>
    public class TvShowsRatingsOperations : BaseRatingsOperations<TvShow, RatingsTvShow, string>, ITvShowsRatingsOperations 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowsRatingsOperations"/> class.
        /// </summary>
        /// <param name="ratingsRepository">
        /// The ratings repository.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public TvShowsRatingsOperations(ITvShowRatingsRepository ratingsRepository, ITvShowsRepository repository)
            : base(ratingsRepository, repository)
        {
        }

        /// <summary>
        /// The get average rating.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public double GetAverageRating(string key)
        {
            var ratings = this.GetAllRatings(key).Ratings;
            return ratings.Count == 0 ? 0 : ratings.Average(rating => rating.UserRating);
        }
    }
}