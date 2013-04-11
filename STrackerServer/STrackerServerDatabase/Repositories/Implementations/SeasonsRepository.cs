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

    class SeasonsRepository : BaseRepository<Season, string>, ISeasonsRepository
    {
        private readonly ITvShowsRepository tvShowsRepository;

        public SeasonsRepository(ITvShowsRepository tvShowsRepository)
        {
            this.tvShowsRepository = tvShowsRepository;
        }

        public override bool Create(Season entity)
        {
            // Get the collection associated to the television show of the season.
            var collection = Database.GetCollection(entity.TvShowId);

            var tvshow = tvShowsRepository.Read(entity.TvShowId);

            if (tvshow == null)
            {
                return false;
            }

            collection.Insert(entity);

            tvshow.SeasonSynopses.Add(entity.GetSynopsis());

            return tvShowsRepository.Update(tvshow);
        }

        public override bool Update(Season entity)
        {
            var collection = Database.GetCollection(entity.TvShowId);

            return collection.Save(entity).Ok;
        }

        public override bool Delete(string id)
        {
            var collection = this.GetCollection(id);

            var query = Query<Season>.EQ(s => s.Id, id);
            
            return collection.FindAndRemove(query, SortBy.Null).Ok;
        }
    }
}
