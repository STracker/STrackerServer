// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowCalendar.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of user's tvshow calendar object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities
{
    using System.Collections.Generic;

    /// <summary>
    /// The user calendar.
    /// </summary>
    public class TvShowCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowCalendar"/> class.
        /// </summary>
        public TvShowCalendar()
        {
            this.Entries = new List<TvShowCalendarTvShowEntry>();  
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        public List<TvShowCalendarTvShowEntry> Entries { get; set; }

        /// <summary>
        /// The television show calendar entry.
        /// </summary>
        public class TvShowCalendarTvShowEntry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TvShowCalendarTvShowEntry"/> class.
            /// </summary>
            public TvShowCalendarTvShowEntry()
            {
                this.Episodes = new List<Episode.EpisodeSynopsis>();
            }

            /// <summary>
            /// Gets or sets the television show.
            /// </summary>
            public TvShow.TvShowSynopsis TvShow { get; set; }

            /// <summary>
            /// Gets or sets the episodes.
            /// </summary>
            public List<Episode.EpisodeSynopsis> Episodes { get; set; }
        }
    }
}
