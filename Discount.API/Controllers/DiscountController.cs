using Discount.API.Entities;
using Discount.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class DiscountController : ControllerBase
	{
		#region Dependency Injection
		private readonly IDiscountRepository _discountRepository;

		#endregion

		#region Ctor
		public DiscountController(IDiscountRepository discountRepository)
		{
			_discountRepository = discountRepository;
		}
		#endregion



		[HttpGet]
		public async Task<IActionResult> GetDiscount(string productName)
		{
			var res =await _discountRepository.GetDiscountasync(productName);
			return Ok(res);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateDiscount(Coupon coupon)
		{
			var res =await _discountRepository.UpdateDiscountasync(coupon);
			return Ok(res);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteDiscount(string productName)
		{
			var res =await _discountRepository.DeleteDiscountasync(productName);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> createDiscount(Coupon coupon)
		{
			var res = await _discountRepository.CreateDiscountasync(coupon);
			return Ok(res);
		}
	}
}
