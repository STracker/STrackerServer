// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseLocator.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Core
{
    using System.Configuration;
    using System.Web.Configuration;

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
        /// The mongo database instance.
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
                /*
                var configSection = WebConfigurationManager.OpenWebConfiguration(null);

                var client = configSection.AppSettings.Settings["MongoDBClient"].Value;

                var database = configSection.AppSettings.Settings["MongoDBName"].Value;
                 */

                Value = new MongoClient("mongodb://localhost").GetServer().GetDatabase("test");
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