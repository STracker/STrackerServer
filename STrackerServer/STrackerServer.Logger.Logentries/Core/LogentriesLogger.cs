// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogentriesLogger.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the LogentriesLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Logger.Logentries.Core
{
    using NLog;

    using STrackerServer.Logger.Core;

    /// <summary>
    /// The logentries logger.
    /// </summary>
    /// <typeparam name="T">
    /// The class type for log.
    /// </typeparam>
    public class LogentriesLogger<T> : BaseLogger<T> where T : class
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogentriesLogger{T}"/> class.
        /// </summary>
        public LogentriesLogger()
        {
            this.logger = LogManager.GetLogger(this.ClassName);
        }

        /// <summary>
        /// Send debug log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Debug(string message)
        {
            this.logger.Debug(message);
        }

        /// <summary>
        /// Send info log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Info(string message)
        {
            this.logger.Info(message);
        }

        /// <summary>
        /// Send warn log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Warn(string message)
        {
            this.logger.Warn(message);
        }

        /// <summary>
        /// Send error log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Error(string message)
        {
            this.logger.Error(message);
        }

        /// <summary>
        /// The fatal log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected override void Fatal(string message)
        {
            this.logger.Fatal(message);
        }
    }
}
