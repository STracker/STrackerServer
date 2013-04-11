using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServerDatabase.Repositories
{
    using STrackerServerDatabase.Core;
    using STrackerServerDatabase.Models;

    interface IEpisodesRepository : IRepository<Episode, string>
    {
        // TODO, add additional stuff...
    }
}
