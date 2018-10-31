using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Search(Func<T, bool> predicate);

        IEnumerable<T> GetAll();

        T Get(int id);

        T Add(T Item);

        void Update(T Item);

        void Delete(T Item);

    }
}
