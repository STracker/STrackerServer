// --------------------------------------------------------------------------------------------------------------------
// <copyright file="STrackerDatabaseException.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   The STracker database exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.DataAccessLayer.Exception
{
    using System;

    /// <summary>
    /// The STracker database exception.
    /// </summary>
    public class STrackerDatabaseException : ApplicationException
    {
        /// <summary>
        /// The default exception message.
        /// </summary>
        private const string DefaultMessage = "An exception has occurred while trying to access or modify information in the database.";

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerDatabaseException"/> class.
        /// </summary>
        public STrackerDatabaseException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STrackerDatabaseException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The database exception.
        /// </param>
        public STrackerDatabaseException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
