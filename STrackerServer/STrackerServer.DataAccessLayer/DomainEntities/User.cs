using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServer.DataAccessLayer.DomainEntities
{
    public class User : Person
    {
        public User(string key) : base(key)
        {
        }

        public List<UserSynopsis> Friends { get; set; }

        public UserSynopsis GetSynopsis()
        {
            return new UserSynopsis { Id = this.Id, Name = this.Name };
        }

        public class UserSynopsis
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
