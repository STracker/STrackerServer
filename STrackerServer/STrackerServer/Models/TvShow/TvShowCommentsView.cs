﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCommentsView.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the TvShowCommentsView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Models.TvShow
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;
    using STrackerServer.Models.Partial;

    /// <summary>
    /// The television show comments view.
    /// </summary>
    public class TvShowCommentsView
    {
        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public TvShowCommentsOptionsView Options { get; set; }
    }
}