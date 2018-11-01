using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using ProductCatalog.Controllers;
using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace ProductCatalogTests
{
	public class ProductCatalogTest
	{
		private Mock<IProductRepository> _productRepository;
		private Mock<IHostingEnvironment> _hostingRepository;
		private Mock<IConfiguration> _configuration;
		private ProductController _productController;
		private List<Product> productList;

		public ProductCatalogTest()
		{
			_productRepository = new Mock<IProductRepository>();
			_hostingRepository = new Mock<IHostingEnvironment>();
			_configuration = new Mock<IConfiguration>();
			_productController = new ProductController(_productRepository.Object, _hostingRepository.Object,_configuration.Object);
		}

		[Fact]
		public void TestGet()
		{
			var testProducts = GetProductList();
			_productRepository.Setup(x => x.GetAll()).Returns(testProducts);
			
			var result = _productController.Get();

			var actionResult=Assert.IsType<OkObjectResult>(result);
			var resultModel = Assert.IsAssignableFrom<IList<Product>>(actionResult.Value);
			Assert.True(testProducts.Count.Equals(resultModel.Count));
		}

		[Fact]
		public void TestGetById()
		{
			var testProduct = GetProduct();
			_productRepository.Setup(x => x.Get(1)).Returns(testProduct);

			var result = _productController.Get(1);

			var actionResult = Assert.IsType<OkObjectResult>(result);
			var resultModel = Assert.IsAssignableFrom<Product>(actionResult.Value);
			Assert.True(testProduct.Equals(resultModel));
		}

		[Fact]
		public void TestAdd()
		{
			var testProduct = GetProduct();
			_productRepository.Setup(x => x.Add(testProduct)).Returns(testProduct);

			var result = _productController.Post(testProduct);

			var actionResult = Assert.IsType<OkObjectResult>(result);
			Assert.True(actionResult.StatusCode.Equals(200));
		}

		[Fact]
		public void TestDelete()
		{
			var testProduct = GetProduct();
			_productRepository.Setup(x => x.Get(1)).Returns(testProduct);

			var result = _productController.Delete(1);

			var actionResult = Assert.IsType<OkResult>(result);
			Assert.True(actionResult.StatusCode.Equals(200));
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
