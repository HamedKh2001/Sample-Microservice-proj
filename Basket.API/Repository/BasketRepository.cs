using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repository
{
	public class BasketRepository : IBasketRepository
	{
		#region Dipendency Injection
		private readonly IDistributedCache _distributedCache;
		#endregion

		#region Ctor
		public BasketRepository(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}
		#endregion

		#region IBasketRepository
		public async Task DeleteBasketasync(string userName)
		{
			await _distributedCache.RemoveAsync(userName);
		}

		public async Task<ShoppingCart?> GetBasketasync(string userName)
		{
			var basket = await _distributedCache.GetStringAsync(userName);
			if (basket == null)
				return null;
			return JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart?> UpdateBasketasync(ShoppingCart basket)
		{
			var serilizedObject = JsonConvert.SerializeObject(basket);
			await _distributedCache.SetStringAsync(basket.UserName, serilizedObject);
			return await GetBasketasync(basket.UserName);
		}
		#endregion
	}
}
