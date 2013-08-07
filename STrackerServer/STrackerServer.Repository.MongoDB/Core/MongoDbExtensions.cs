// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongodbExtensions.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of mongo DB extensions methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.Repository.MongoDB.Core
{
    using System.Linq;

    using global::MongoDB.Driver;
    using global::MongoDB.Driver.Builders;

    /// <summary>
    /// The mongo DB extensions.
    /// </summary>
    public static class MongodbExtensions
    {
        /// <summary>
        /// The find one.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="excludedFields">
        /// The excluded fields.
        /// </param>
        /// <typeparam name="T">
        /// Type of the document object.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T FindOne<T>(this MongoCollection collection, IMongoQuery query, params string[] excludedFields)
        {
            return collection.FindAs<T>(query).SetLimit(1).SetFields(Fields.Exclude(excludedFields)).FirstOrDefault();
        }
    }
}