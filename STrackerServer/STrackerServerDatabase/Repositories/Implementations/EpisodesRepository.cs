using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServerDatabase.Repositories.Implementations
{
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using STrackerServerDatabase.Models;

    class EpisodesRepository : BaseRepository<Episode, string>, IEpisodesRepository
    {
        private readonly ISeasonsRepository seasonsRepository;

        public EpisodesRepository(ISeasonsRepository seasonsRepository)
        {
            this.seasonsRepository = seasonsRepository;
        }

        public override bool Create(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            var season = seasonsRepository.Read(string.Format("{0},{1}",entity.TvShowId, entity.SeasonNumber));

            if (season == null)
            {
                return false;
            }

            collection.Insert(entity);
            
            season.EpisodeSynopses.Add(entity.GetSynopsis());

            return seasonsRepository.Update(season);
        }

        public override bool Update(Episode entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            if(!collection.Save(entity).Ok)
            {
                return false;
            }

            var season = seasonsRepository.Read(string.Format("{0},{1}", entity.TvShowId, entity.SeasonNumber));

            var synopse = season.EpisodeSynopses.FirstOrDefault(s => s.Number == entity.Number);

            season.EpisodeSynopses.Remove(synopse);

            synopse = entity.GetSynopsis();

            season.EpisodeSynopses.Add(synopse);

            return seasonsRepository.Update(season);
        }

        public override bool Delete(string id)
        {
            var collection = this.GetCollection(id);

            var query = Query<Episode>.EQ(s => s.Id, id);

            return collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
