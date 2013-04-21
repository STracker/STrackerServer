using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STrackerServer.BusinessLayer.Core;
using STrackerServer.DataAccessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;

namespace STrackerServer.BusinessLayer.Operations
{
    public class UsersOperations : BaseCrudOperations<User,string>, IUsersOperations
    {
        public UsersOperations(IUsersRepository repository) : base(repository)
        {
        }

        public override bool Create(User entity)
        {
            return Repository.Create(entity);
        }

        public override bool Update(User entity)
        {
            return Repository.Update(entity);
        }
    }
}
