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
                var client = ConfigurationManager.AppSettings["MongoDBClient"];

                var database = ConfigurationManager.AppSettings["MongoDBName"];
                 
                Value = new MongoClient(client).GetServer().GetDatabase(database);
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