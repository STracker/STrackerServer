// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the ILogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Logger.Core
{
    /// <summary>
    /// The Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Send debug log.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Debug(string method, string exception, string message);

        /// <summary>
        /// Send info log.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Info(string method, string exception, string message);

        /// <summary>
        /// Send warn log.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Warn(string method, string exception, string message);

        /// <summary>
        /// Send error log.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Error(string method, string exception, string message);

        /// <summary>
        /// The fatal log.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Fatal(string method, string exception, string message);
    }
}
