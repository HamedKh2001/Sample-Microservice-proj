using Discount.Grpc.Entities;

namespace Discount.Grpc.Repository
{
	public interface IDiscountRepository
	{
		Task<Coupon> GetDiscountasync(string productName);
		Task<bool> CreateDiscountasync(Coupon coupon);
		Task<bool> UpdateDiscountasync(Coupon coupon);
		Task<bool> DeleteDiscountasync(string productName);
	}
}
