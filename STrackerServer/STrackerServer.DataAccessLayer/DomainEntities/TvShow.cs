﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShow.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of television show domain entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// Television show domain entity.
    /// </summary>
    public class TvShow : Media
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public TvShow(string id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the air day.
        /// </summary>
        public string AirDay { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public int Runtime { get; set; }

        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        public Person Creator { get; set; }

        /// <summary>
        /// Gets or sets the season synopses.
        /// </summary>
        public List<Season.SeasonSynopsis> SeasonSynopses { get; set; }

        /// <summary>
        /// Get the television show synopsis.
        /// </summary>
        /// <returns>
        /// The <see cref="TvShowSynopsis"/>.
        /// </returns>
        public TvShowSynopsis GetSynopsis()
        {
            return new TvShowSynopsis { Id = this.Id, Name = this.Name };
        }

        /// <summary>
        /// Television show synopsis object.
        /// </summary>
        public class TvShowSynopsis
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            public string Name { get; set; }
        }
    }
}