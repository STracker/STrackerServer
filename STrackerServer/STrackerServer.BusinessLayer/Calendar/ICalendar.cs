// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICalendar.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Generates a iCalendar from the new episodes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Calendar
{
    using System.Collections.Generic;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The Calendar interface.
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// Creates the calendar.
        /// </summary>
        /// <param name="calendars">
        /// The television show's episode calendars.
        /// </param>
        /// <returns>
        /// The calendar file in bytes
        /// </returns>
        byte[] Create(ICollection<TvShowCalendar> calendars);
    }
}
