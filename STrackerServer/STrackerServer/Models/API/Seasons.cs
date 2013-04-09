
namespace STrackerServer.Models.API
{
    using System.Collections.Generic;

    /// <summary>
    /// The seasons.
    /// </summary>
    public class Seasons
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Seasons"/> class.
        /// </summary>
        public Seasons()
        {
            this.List = new List<int>();
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        public List<int> List { get; private set; }  
    }
}