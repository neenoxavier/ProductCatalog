using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
		private readonly ProductDBContext _productDBContext;

		public ProductRepository(ProductDBContext productDBContext):base(productDBContext)
        {
			_productDBContext = productDBContext;
		}
        public Product GetProductCatalog()
        {
            throw new NotImplementedException();
        }
    }
}
