using ProductCatalog.Data.Models;

namespace ProductCatalog.Data.Interfaces
{
	public interface IProductRepository:IRepository<Product>
    {
        // Add Product Specific Functions.
    }
}

