using ProductCatalog.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Repository
{
	/// <summary>
	/// Repository for Generic functions
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T:class
    {
        private ProductDBContext _productDBContext;

        public Repository(ProductDBContext productDBContext)
        {
            _productDBContext = productDBContext;
        }

        public T Add(T Item)
        {
            _productDBContext.Add(Item);
            _productDBContext.SaveChanges();
			return Item;
        }

        public void Delete(T Item)
        {
            _productDBContext.Remove(Item);
            _productDBContext.SaveChanges();
        }

        public T Get(int id)
        {
            return _productDBContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _productDBContext.Set<T>();
        }

        public IEnumerable<T> Search(Func<T,bool> predicate)
        {
            return _productDBContext.Set<T>().Where(predicate);
        }

        public void Update(T Item)
        {
            _productDBContext.Update(Item);
            _productDBContext.SaveChanges();
        }
    }
}
