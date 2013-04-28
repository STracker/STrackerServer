namespace STrackerServer.Models.TvShow
{
    using System.Collections.Generic;
    using DataAccessLayer.DomainEntities;

    /// <summary>
    /// The television show episodes info.
    /// </summary>
    public class TvShowEpisodesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowEpisodesInfo"/> class.
        /// </summary>
        public TvShowEpisodesInfo()
        {
            this.List = new List<IEnumerable<Episode.EpisodeSynopsis>>();
        }

        /// <summary>
        /// Gets or sets the television show id.
        /// </summary>
        public string TvShowId { get; set; }

        /// <summary>
        /// Gets the list.
        /// </summary>
        public IList<IEnumerable<Episode.EpisodeSynopsis>> List { get; private set; }

        /// <summary>
        /// Gets or sets the number of seasons.
        /// </summary>
        public int NumberOfSeasons { get; set; }
    }
}