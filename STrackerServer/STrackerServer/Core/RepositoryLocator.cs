// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryLocator.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Core
{
    using STrackerServerDatabase.Repositories;

    /// <summary>
    /// The repository locator.
    /// </summary>
    public class RepositoryLocator
    {
        /// <summary>
        /// The television show repository.
        /// </summary>
        private static readonly TvShowRepository TelevisionShowsRepositoryValue = new TvShowRepository();

        /// <summary>
        /// Gets the television shows repository.
        /// </summary>
        public static TvShowRepository TelevisionShowsRepository
        {
            get
            {
                return TelevisionShowsRepositoryValue;
            }
        }
    }
}