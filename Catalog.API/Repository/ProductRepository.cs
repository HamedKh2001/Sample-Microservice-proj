using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repository
{
	public class ProductRepository : IProductRepository
	{
		#region Properties
		public ICatalogContext _catalogContext { get; set; }
		#endregion

		#region Ctor
		public ProductRepository(ICatalogContext catalogContext)
		{
			_catalogContext = catalogContext;
		}
		#endregion

		#region IProductRepository
		public async Task<bool> CreateProductAsync(Product product)
		{
			await _catalogContext
				.Products
				.InsertOneAsync(product);
			return true;
		}

		public async Task<bool> DeleteProductAsync(string id)
		{
			//FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
			await _catalogContext
				.Products
				.DeleteOneAsync(filter: p => p.Id == id);
			return true;
		}

		public async Task<Product> GetProductByIdAsync(string id)
		{
			return await _catalogContext
				.Products
				.Find(p => p.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
		{
			return await _catalogContext
				.Products
				.Find(p => p.Name == name)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
		{
			return await _catalogContext
				.Products
				.Find(p => p.Category == category)
				.ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			return await _catalogContext
				.Products
				.Find(p => true)
				.ToListAsync();
		}

		public async Task<bool> UpdateProductAsync(Product product)
		{
			var updateRes = await _catalogContext
					.Products
					.ReplaceOneAsync(p => p.Id == product.Id, product);

			return updateRes.IsAcknowledged &&
				updateRes.ModifiedCount > 0;
		}
		#endregion
	}
}
