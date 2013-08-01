// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Suggestion.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of suggestion object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    /// <summary>
    /// The suggestion.
    /// </summary>
    public class Suggestion
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public User.UserSynopsis User { get; set; }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public TvShow.TvShowSynopsis TvShow { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
