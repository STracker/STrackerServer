using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServerBusinessLayer.Facades
{
    using STrackerServerBusinessLayer.Core;

    using STrackerServerDatabase.Core;

    public abstract class BaseCrudFacade<T, TK> : ICrudOperations<T, TK> where T : IEntity<TK>
    {
        protected IRepository<T, TK> _repository;

        protected BaseCrudFacade(IRepository<T, TK> repository)
        {
            _repository = repository;
        }

        public T Read(TK id)
        {
            return _repository.Read(id);
        }

        public bool Update(T entity)
        {
            return !this._repository.Read(entity.Id).Equals(default(T)) && this._repository.Update(entity);
        }

        public bool Delete(TK id)
        {
            return !this._repository.Read(id).Equals(default(T)) && this._repository.Delete(id);
        }

        public abstract bool Create(T entity);
    }
}
