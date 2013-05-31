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
    public class EpisodeComments : Container<Tuple<string, int, int>, Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeComments"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public EpisodeComments(Tuple<string, int, int> key)
            : base(key, "Comments")
        {
        }
    }
}
