// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Calendar.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The iCal calendar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Calendar
{
    using System;
    using System.Collections.Generic;

    using DDay.iCal;
    using DDay.iCal.Serialization.iCalendar;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The iCal calendar.
    /// </summary>
    public class Calendar : BaseCalendar
    {
        /// <summary>
        /// The generate calendar.
        /// </summary>
        /// <param name="calendars">
        /// The calendars.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string GenerateCalendar(ICollection<TvShowCalendar> calendars)
        {
            var cal = new iCalendar();

            foreach (var calendar in calendars)
            {
                foreach (var entry in calendar.Entries)
                {
                    foreach (var episode in entry.Episodes)
                    {
                        DateTime date;
                        if (!DateTime.TryParse(episode.Date, out date))
                        {
                            continue;
                        }

                        var evt = cal.Create<Event>();
                        evt.Start = new iCalDateTime(date);
                        evt.Summary = string.Format("{0} Season {1} Episode {2}", entry.TvShow.Name, episode.Id.SeasonNumber, episode.Id.EpisodeNumber);
                        evt.Description = string.Format("Name: {0}.", episode.Name);
                    }
                }
            }

            return new iCalendarSerializer().SerializeToString(cal);
        }
    }
}
