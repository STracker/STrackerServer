// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoUtils.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Object that holds the MongoDB thread-safe objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Configuration;

    using global::MongoDB.Driver;

    /// <summary>
    /// Mongo utils.
    /// </summary>
    internal class MongoUtils
    {
        /// <summary>
        /// Initializes static members of the <see cref="MongoUtils"/> class.
        /// </summary>
        static MongoUtils()
        {
            Url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);

            Client = new MongoClient(Url);

            Server = Client.GetServer();

            Database = Server.GetDatabase(Url.DatabaseName);
        }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public static MongoUrl Url { get; private set; }

        /// <summary>
        /// Gets the client.
        /// </summary>
        public static MongoClient Client { get; private set; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        public static MongoServer Server { get; private set; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        public static MongoDatabase Database { get; private set; }
    }
}
