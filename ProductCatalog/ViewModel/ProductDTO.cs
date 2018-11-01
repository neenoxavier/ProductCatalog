using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.ViewModel
{
	public class ProductDTO
	{
		public ProductDTO(int id,string name,decimal price,DateTime lastUpdated)
		{
			this.ProductId = id;
			this.ProductName = name;
			this.ProductPrice = price;
			this.ProductLastUpdated = lastUpdated;
		}

		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal ProductPrice { get; set; }
		public DateTime ProductLastUpdated { get; set; }
	}
}
