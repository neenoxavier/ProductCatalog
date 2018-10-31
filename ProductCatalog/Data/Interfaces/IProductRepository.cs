using ProductCatalog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Interfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductCatalog();
    }
}

