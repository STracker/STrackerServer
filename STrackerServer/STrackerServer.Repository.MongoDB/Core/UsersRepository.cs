using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using STrackerServer.DataAccessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;

namespace STrackerServer.Repository.MongoDB.Core
{
    public class UsersRepository : BaseRepository<User,string>, IUsersRepository
    {
        private readonly MongoCollection collection;
  
        public UsersRepository(MongoClient client, MongoUrl url) : base(client, url)
        {
            collection = this.Database.GetCollection<User>("Users");
        }

        public override bool Create(User entity)
        {
            return this.collection.Insert(entity).Ok;
        }

        public override User Read(string key)
        {
            return this.collection.FindOneByIdAs<User>(key);
        }

        public override bool Update(User entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(string key)
        {
            throw new NotImplementedException();
        }
    }
}
