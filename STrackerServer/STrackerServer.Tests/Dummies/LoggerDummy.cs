// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerDummy.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the LoggerDummy type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Tests.Dummies
{
    using STrackerServer.Logger.Core;

    /// <summary>
    /// The logger dummy.
    /// </summary>
    public class LoggerDummy : ILogger
    {
        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(string method, string exception, string message)
        {
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string method, string exception, string message)
        {
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(string method, string exception, string message)
        {
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Error(string method, string exception, string message)
        {
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Fatal(string method, string exception, string message)
        {
        }
    }
}
