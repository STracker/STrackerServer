// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeasonsDocumentRepository.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using System;

    using MongoDB.Driver.Builders;

    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    /// <summary>
    /// DocumentRepository of the television shows seasons.
    /// </summary>
    public class SeasonsDocumentRepository : DocumentRepository<Season, Tuple<string, int>>
    {
        /// <summary>
        /// Create a new season.
        /// </summary>
        /// <param name="entity">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Season entity)
        {
            // Get the collection associated to the television show of the season.
            var collection = Database.GetCollection(entity.TvShowId);

            var tvshow = RepositoryLocator.TelevisionShowsDocumentRepository.Read(entity.TvShowId);

            if (tvshow == null)
            {
                return false;
            }

            tvshow.SeassonSynopses.Add(entity.GetSynopsis());

            collection.Insert(entity);

            return RepositoryLocator.TelevisionShowsDocumentRepository.Update(tvshow);
        }

        /// <summary>
        /// Get the season.
        /// </summary>
        /// <param name="id">
        /// The id. Composed with television show id plus season number.
        /// </param>
        /// <returns>
        /// The <see cref="Season"/>.
        /// </returns>
        public override Season Read(Tuple<string, int> id)
        {
            // Get the collection associated to the television show of the season.
            var collection = Database.GetCollection(id.Item1);

            return collection.FindOneByIdAs<Season>(string.Format("{0}_{1}", id.Item1, id.Item2));
        }

        /// <summary>
        /// Update the desire season.
        /// </summary>
        /// <param name="entity">
        /// The season.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Season entity)
        {
            // Get the collection associated to the television show of the season.
            var collection = Database.GetCollection(entity.TvShowId);

            var query = Query<Season>.EQ(s => s.Id, entity.Id);
            
            return collection.Remove(query).Ok;
        }

        /// <summary>
        /// Delete the season.
        /// </summary>
        /// <param name="id">
        /// The id. Composed with television show id plus season number.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(Tuple<string, int> id)
        {
            // Get the collection associated to the television show of the season.
            var collection = Database.GetCollection(id.Item1);

            var query = Query<Season>.EQ(s => s.Id, string.Format("{0}_{1}", id.Item1, id.Item2));

            return collection.Remove(query).Ok;
        }
    }
}
