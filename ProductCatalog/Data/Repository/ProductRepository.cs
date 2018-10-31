using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;

namespace ProductCatalog.Data.Repository
{
	public class ProductRepository : Repository<Product>,IProductRepository
    {
		private readonly ProductDBContext _productDBContext;

		public ProductRepository(ProductDBContext productDBContext):base(productDBContext)
        {
			_productDBContext = productDBContext;
		}
        
		// Add Product specific functionalities
    }
}
