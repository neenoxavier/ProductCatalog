using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;
using System;
using Xunit;

namespace ProductCatalogTests
{
	public class ProductServiceTest
	{
		private readonly IProductRepository _productRepository;

		public ProductServiceTest(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[Fact]
		public void TestGet() {
			var productItems=_productRepository.GetAll();
			Assert.NotEmpty(productItems);
		}
		[Fact]
		public void TestGetById()
		{
			var productItem = _productRepository.Get(1);
			Assert.NotNull(productItem);
		}
		[Fact]
		public void TestSearch()
		{
			var productItems = _productRepository.Search(item=>item.Name.ToUpper().Contains("A"));
			Assert.NotEmpty(productItems);
		}
		[Fact]
		public void TestUpdate()
		{
			Product product = new Product();
			product.Name = "Test";
			var productId =(_productRepository.Add(product));
			Assert.NotNull(productId);
		}
	}
}
