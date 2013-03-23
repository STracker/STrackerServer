using System.Collections.Generic;
using STrackerServer.Models;

namespace STrackerServer.Testing_Repository
{
    public class TvShowsTestRepository
    {  
        private readonly IDictionary<int, TvShow> _dictionary = new Dictionary<int, TvShow>();

        public TvShowsTestRepository()
        {
            var tvshow = new TvShow
                {
                    ImdbId = 1,
                    Title = "The Walking Dead",
                    Description = "zombies apocalypse!!!",
                    Runtime = 45,
                    Rating = 4
                };

            _dictionary.Add(1, tvshow);
        }

        public void Add(TvShow tvShow)
        {
            _dictionary.Add(tvShow.ImdbId, tvShow);
        }

        public TvShow Get(int id)
        {
            TvShow tvshow;
            _dictionary.TryGetValue(id, out tvshow);
            return tvshow;
        }
    }
}