using Basket.API.Entities;

namespace Basket.API.Repository
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketasync(string userName);
        Task<ShoppingCart?> UpdateBasketasync(ShoppingCart basket);
        Task DeleteBasketasync(string userName);
    }
}
