// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCalendar.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The base calendar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.BusinessLayer.Calendar
{
    using System.Collections.Generic;
    using System.Text;

    using STrackerServer.DataAccessLayer.DomainEntities.AuxiliaryEntities;

    /// <summary>
    /// The base calendar.
    /// </summary>
    public abstract class BaseCalendar : ICalendar
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
        public byte[] Create(ICollection<TvShowCalendar> calendars)
        {
            return Encoding.UTF32.GetBytes(this.GenerateCalendar(calendars));
        }

        /// <summary>
        /// The generate calendar.
        /// </summary>
        /// <param name="calendars">
        /// The calendars.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public abstract string GenerateCalendar(ICollection<TvShowCalendar> calendars);
    }
}
