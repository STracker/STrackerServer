using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STrackerServer.DataAccessLayer.DomainEntities;

namespace STrackerServer.DataAccessLayer.Core
{
    public interface IUsersRepository : IRepository<User, string>
    {
        // TODO
    }
}
