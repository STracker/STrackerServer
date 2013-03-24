using System;

namespace STrackerServer.Models
{
    public class Media
    {
        public int ImdbId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public List<Artwork> Artworks { get; set; } 
        //public List<Actor> Actors { get; set; } 
        public int Rating { get; set; }
    }


    public class TvShow : Media
    {
        public DateTime FirstAired { get; set; }
        public int Runtime { get; set; }
        //public Person Creator { get; set; }
        //public List<Genre> Genres { get; set; } 
    }
}