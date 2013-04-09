// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryLocator.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Core
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
        private static readonly TvShowsDocumentRepository TelevisionShowsDocumentRepositoryValue = new TvShowsDocumentRepository();

        /// <summary>
        /// The seasons repository value.
        /// </summary>
        private static readonly SeasonsDocumentRepository SeasonsDocumentRepositoryValue = new SeasonsDocumentRepository();

        /// <summary>
        /// Gets the television shows repository.
        /// </summary>
        public static TvShowsDocumentRepository TelevisionShowsDocumentRepository
        {
            get
            {
                return TelevisionShowsDocumentRepositoryValue;
            }
        }

        /// <summary>
        /// Gets the seasons repository.
        /// </summary>
        public static SeasonsDocumentRepository SeasonsDocumentRepository
        {
            get
            {
                return SeasonsDocumentRepositoryValue;
            }
        }
    }
}