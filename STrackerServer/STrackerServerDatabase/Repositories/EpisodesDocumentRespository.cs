// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EpisodesDocumentRespository.cs" company="STracker">
//   Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServerDatabase.Repositories
{
    using System;
    using Core;
    using Models;
    using MongoDB.Driver.Builders;

    /// <summary>
    /// The episodes document repository.
    /// </summary>
    public class EpisodesDocumentRepository : DocumentRepository<Episode,Tuple<string,int,int>>
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Create(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            var season = RepositoryLocator.SeasonsDocumentRepository.Read(new Tuple<string, int>(entity.TvShowId,entity.SeasonNumber));

            if (season == null)
            {
                return false;
            }

            season.EpisodeSynopses.Add(entity.GetSynopsis());

            collection.Insert(entity);

            return RepositoryLocator.SeasonsDocumentRepository.Update(season);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Episode"/>.
        /// </returns>
        public override Episode Read(Tuple<string, int, int> id)
        {
            var collection = Database.GetCollection(id.Item1);

            return collection.FindOneByIdAs<Episode>(string.Format("{0}_{1}_{2}", id.Item1, id.Item2, id.Item3));
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Update(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            return collection.Save(entity).Ok;
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Delete(Tuple<string, int, int> id)
        {
            var collection = Database.GetCollection(id.Item1);

            var query = Query<Episode>.EQ(s => s.Id, string.Format("{0}_{1}_{2}", id.Item1, id.Item2, id.Item3));

            // Encontra o primeiro e remove, o outro procurava em todos os documentos mesmo depois de ter encontrado um.
            return collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
