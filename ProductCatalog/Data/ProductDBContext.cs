using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Data
{ 
	/// <summary>
	/// Product DB Context for Entity Framework
	/// </summary>
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; } 
    }
}
