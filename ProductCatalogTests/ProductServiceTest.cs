using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using ProductCatalog.Controllers;
using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace ProductCatalogTests
{
	public class ProductServiceTest
	{
		private Mock<IProductRepository> _productRepository;
		private Mock<IHostingEnvironment> _hostingRepository;
		private Mock<IConfiguration> _configuration;
		private ProductController _productController;
		private List<Product> productList;

		public ProductServiceTest()
		{
			_productRepository = new Mock<IProductRepository>();
			_hostingRepository = new Mock<IHostingEnvironment>();
			_configuration = new Mock<IConfiguration>();
			_productController = new ProductController(_productRepository.Object, _hostingRepository.Object,_configuration.Object);
		}

		[Fact]
		public void TestGet() {
			_productRepository.Setup(x => x.GetAll()).Returns(GetProductList());
			var list=_productController.Get();
			Assert.NotEmpty(list);
		}
		[Fact]
		public void TestGetById()
		{
			_productRepository.Setup(x => x.Get(1)).Returns(GetProduct);
			var item = _productController.Get(1);
			Assert.NotNull(item);
		}

		public List<Product> GetProductList() {
			productList = new List<Product>();
			productList.Add(new Product
			{
				Id = 1,
				Name = "Test1",
				Photo = "Test.jpg",
				Price = 100,
				LastUpdated = DateTime.Now
			});
			return productList;
		}

		public Product GetProduct() {
			return new Product
			{
				Id = 1,
				Name = "Test1",
				Photo = "Test.jpg",
				Price = 100,
				LastUpdated = DateTime.Now
			};
		}
	}
}
