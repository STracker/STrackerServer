// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseLogger.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Logger.Core
{
    /// <summary>
    /// The abstract base logger.
    /// </summary>
    /// <typeparam name="T">
    /// The class type for log.
    /// </typeparam>
    public abstract class BaseLogger<T> : ILogger<T>
    {
        /// <summary>
        /// The class name.
        /// </summary>
        protected readonly string ClassName = typeof(T).Name;

        /// <summary>
        /// The log format.
        /// </summary>
        private const string LogFormat = "Class: {0} - Method: {1} - Exception: {2} - Message: {3}";

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
        public void Debug(string method, string exception, string message)
        {
            this.Debug(string.Format(LogFormat, this.ClassName, method, exception, message));
        }

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
        public void Info(string method, string exception, string message)
        {
            this.Info(string.Format(LogFormat, this.ClassName, method, exception, message));
        }

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
        public void Warn(string method, string exception, string message)
        {
            this.Warn(string.Format(LogFormat, this.ClassName, method, exception, message));
        }

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
        public void Error(string method, string exception, string message)
        {
            this.Error(string.Format(LogFormat, this.ClassName, method, exception, message));
        }

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
        public void Fatal(string method, string exception, string message)
        {
            this.Fatal(string.Format(LogFormat, this.ClassName, method, exception, message));
        }

        /// <summary>
        /// Send debug log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected abstract void Debug(string message);

        /// <summary>
        /// Send info log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected abstract void Info(string message);

        /// <summary>
        /// Send warn log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected abstract void Warn(string message);

        /// <summary>
        /// Send error log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected abstract void Error(string message);

        /// <summary>
        /// The fatal log.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        protected abstract void Fatal(string message);
    }
}
