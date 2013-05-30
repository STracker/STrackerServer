// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodeComments.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The episode comments.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System;

    /// <summary>
    /// The episode comments.
    /// </summary>
    public class EpisodeComments : Container<Tuple<string, string, int>, Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        public EpisodeComments(Tuple<string, string, int> key, string container)
            : base(key, container)
        {
        }
    }
}
