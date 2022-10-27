using Catalog.API.Entities;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class CatalogController : ControllerBase
	{
		#region Dependency Injection
		public readonly IProductRepository _productRepository;
		public readonly ILogger<CatalogController> _logger;
		#endregion

		#region Ctor
		public CatalogController(IProductRepository productRepository,
			ILogger<CatalogController> logger)
		{
			_productRepository = productRepository;
			_logger = logger;
		}
		#endregion

		[HttpGet]
		public async Task<IActionResult> GetProductById(string id)
		{
			var res = await _productRepository.GetProductByIdAsync(id);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var res = await _productRepository.GetProductsAsync();
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> GetProductByCategory(string category)
		{
			var res = await _productRepository.GetProductsByCategoryAsync(category);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(Product product)
		{
			var res = await _productRepository.CreateProductAsync(product);
			return CreatedAtAction(nameof(GetProductById),product.Id,product);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			var res = await _productRepository.UpdateProductAsync(product);
			return await Task.FromResult(Ok(res));
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			var res = await _productRepository.DeleteProductAsync(id);
			return await Task.FromResult(Ok(res));
		}
	}
}
