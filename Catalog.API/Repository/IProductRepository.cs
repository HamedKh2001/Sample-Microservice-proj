using Catalog.API.Entities;

namespace Catalog.API.Repository
{
	public interface IProductRepository
	{
		Task<IEnumerable<Product>> GetProductsAsync();
		Task<bool> DeleteProductAsync(string id);
		Task<Product> GetProductByIdAsync(string id);
		Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
		Task<bool> UpdateProductAsync(Product product);
		Task<bool> CreateProductAsync(Product product);
		Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
	}
}
