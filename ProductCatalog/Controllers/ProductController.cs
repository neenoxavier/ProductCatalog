using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using ProductCatalog.Data.Interfaces;
using ProductCatalog.Data.Models;
using ProductCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IConfiguration _configuration;

		public ProductController(IProductRepository productRepository, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
		{
			_productRepository = productRepository;
			_hostingEnvironment = hostingEnvironment;
			_configuration = configuration;
		}

		/// <summary>
		/// Get all products
		/// </summary>
		/// <returns>List of Products</returns>
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_productRepository.GetAll());
		}

		/// <summary>
		/// Get product using product id
		/// </summary>
		/// <param name="productId"></param>
		/// <returns>Product for input id</returns>
		[HttpGet("{productId}", Name = "Get")]
		public IActionResult Get(int productId)
		{
			return Ok(_productRepository.Get(productId));
		}

		/// <summary>
		/// Search for a product name. Contains search
		/// </summary>
		/// <param name="token"></param>
		/// <returns>Product for input search token</returns>
		[HttpGet]
		[Route("Search/{token}")]
		public IActionResult Search(string token)
		{
			token = token.ToUpper();
			return Ok(_productRepository.Search(product => product.Name.ToUpper().Contains(token)));
		}

		/// <summary>
		/// Add a new product
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Post(Product product)
		{
			product.LastUpdated = DateTime.Now;
			return Ok((_productRepository.Add(product)).Id);
		}

		/// <summary>
		/// Update existing product
		/// </summary>
		/// <param name="product"></param>
		[HttpPut]
		public IActionResult Put(Product product)
		{
			try
			{
				product.LastUpdated = DateTime.Now;
				_productRepository.Update(product);
				return Ok();
			}
			catch (Exception ex)
			{
				throw new Exception(ErrorMessage, ex);
			}
		}

		/// <summary>
		/// Delete product
		/// </summary>
		/// <param name="id"></param>
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				Product product = _productRepository.Get(id);
				if (product!=null)
				{
					string filePath = Path.Combine(_hostingEnvironment.ContentRootPath + _configuration["ImageRepository"] + product.Photo);
					_productRepository.Delete(product);
					FileInfo file = new FileInfo(filePath);
					if (file.Exists)
						file.Delete();
					return Ok();
				}
				else
					return NotFound();
			}
			catch (Exception ex)
			{
				throw new Exception(ErrorMessage, ex);
			}
		}

		/// <summary>
		/// Export product catalog to excel
		/// </summary>
		/// <returns>Product Catalog Excel</returns>
		[HttpGet]
		[Route("ExportProduct")]
		public async Task<IActionResult> ExportProduct()
		{
			try
			{
				string rootFolder = _hostingEnvironment.ContentRootPath;
				string fileName = @"ProductCatalog_V" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";

				FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

				using (ExcelPackage package = new ExcelPackage(file))
				{
					IList<Product> productList = _productRepository.GetAll().ToList();
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

					worksheet.Cells[1, 1].Value = "Id";
					worksheet.Cells[1, 2].Value = "Product Name";
					worksheet.Cells[1, 3].Value = "Price";
					worksheet.Cells[1, 4].Value = "Last Updated";

					int i = 0;
					int rowCount = productList.Count();
					for (int row = 2; row <= rowCount + 1; row++)
					{
						worksheet.Cells[row, 1].Value = productList[i].Id;
						worksheet.Cells[row, 2].Value = productList[i].Name;
						worksheet.Cells[row, 3].Value = productList[i].Price;
						worksheet.Cells[row, 4].Value = productList[i].LastUpdated.ToLongTimeString();
						i++;
					}

					package.Save();
				}

				var memory = new MemoryStream();
				using (var stream = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Open))
				{
					await stream.CopyToAsync(memory);
				}
				memory.Position = 0;

				file.Delete();
				return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
			catch (Exception ex)
			{
				throw new Exception(ErrorMessage, ex);
			}
		}

		/// <summary>
		/// Upload product photo
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit]
		[Route("UploadPhoto/{productId}")]
		public IActionResult UploadFile(int productId)
		{
			try
			{
				var file = Request.Form.Files[0];
				string newPath = Path.Combine(_hostingEnvironment.ContentRootPath + _configuration["ImageRepository"]);
				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}
				if (file.Length > 0)
				{
					string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
					string fullPath = Path.Combine(newPath, fileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					var productItem = _productRepository.Get(productId);
					productItem.Photo = fileName;
					_productRepository.Update(productItem);
				}
				return Json("Upload Successful.");
			}
			catch (Exception ex)
			{
				throw new Exception(ErrorMessage, ex);
			}
		}

		/// <summary>
		/// Product Catalog API
		/// </summary>
		/// <returns>List of ProductDTO</returns>
		[HttpGet]
		[Route("ProductCatalog")]
		public IActionResult GetProductCatalog()
		{
			IEnumerable<Product> productList = _productRepository.GetAll();
			List<ProductDTO> ProductDTOList = new List<ProductDTO>();
			foreach (var product in productList)
			{
				ProductDTOList.Add(new ProductDTO(product.Id, product.Name, product.Price, product.LastUpdated));
			}
			return Ok(ProductDTOList);
		}

		private const string ErrorMessage = "Something went wrong. Please contact your administrator";
	}
}
