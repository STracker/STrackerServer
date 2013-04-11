using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STrackerServerBusinessLayer.Core
{
    using STrackerServerDatabase.Core;

    public interface ICrudOperations<T, in TK> where T : IEntity<TK>
    {
        bool Create(T entity);

        T Read(TK id);

        bool Update(T entity);

        bool Delete(TK id);
    }
}
