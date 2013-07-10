// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesWatchedView.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episodes watched view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.User
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;

    /// <summary>
    /// The episodes watched view.
    /// </summary>
    public class EpisodesWatchedView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodesWatchedView"/> class.
        /// </summary>
        public EpisodesWatchedView()
        {
            this.List = new List<SubscriptionDetailView>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        public List<SubscriptionDetailView> List { get; set; }

        /// <summary>
        /// Gets or sets the picture url.
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// The subscription detail view.
        /// </summary>
        public class SubscriptionDetailView
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionDetailView"/> class.
            /// </summary>
            public SubscriptionDetailView()
            {
                this.EpisodesWatched = new Dictionary<int, IList<Episode.EpisodeSynopsis>>();
            }

            /// <summary>
            /// Gets or sets the television show.
            /// </summary>
            public TvShow.TvShowSynopsis TvShow { get; set; }

            /// <summary>
            /// Gets or sets the episodes watched.
            /// </summary>
            public IDictionary<int, IList<Episode.EpisodeSynopsis>> EpisodesWatched { get; set; } 
        }
    }
}