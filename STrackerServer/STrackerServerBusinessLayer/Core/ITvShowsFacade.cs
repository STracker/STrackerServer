using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServerBusinessLayer.Core
{
    using STrackerServerDatabase.Models;

    public interface ITvShowsFacade : ICrudOperations<TvShow, string>
    {
        List<TvShow> ReadAllByGenre(Genre genre);

        // Addictional actions...
    }
}
