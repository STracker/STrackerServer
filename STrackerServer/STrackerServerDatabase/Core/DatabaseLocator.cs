// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseLocator.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Core
{
    using System.Configuration;

    using MongoDB.Driver;

    /// <summary>
    /// Database locator.
    /// </summary>
    public class DatabaseLocator
    {
        /// <summary>
        /// Get mongo database instance.
        /// </summary>
        /// <returns>
        /// The <see cref="MongoDatabase"/>.
        /// </returns>
        public static MongoDatabase GetMongoDbDatabaseInstance()
        {
            return MongoDbInstance.DatabaseResource;
        }

        /// <summary>
        /// Lazy initialization holder class for MongoDB database.
        /// </summary>
        private static class MongoDbInstance
        {
            /// <summary>
            /// The database resource.
            /// </summary>
            private static readonly MongoDatabase Value;

            /// <summary>
            /// Initializes static members of the <see cref="MongoDbInstance"/> class.
            /// </summary>
            static MongoDbInstance()
            {
                var url = new MongoUrl(ConfigurationManager.AppSettings["MongoDBURL"]);

                Value = new MongoClient(url).GetServer().GetDatabase(url.DatabaseName);
            }

            /// <summary>
            /// Gets the database resource.
            /// </summary>
            public static MongoDatabase DatabaseResource
            {
                get
                {
                    return Value;
                }
            }
        }
    }
}