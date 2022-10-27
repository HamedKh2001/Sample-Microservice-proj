using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repository;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class BasketController : ControllerBase
	{
		#region Dipendency Injection
		private readonly IBasketRepository _basketRepository;
		private readonly DiscountGrpcService _discountGrpcService;
		private readonly IMapper _mapper;
		private readonly IPublishEndpoint _publishEndpoint;
		#endregion

		#region Ctor
		public BasketController(IMapper mapper,IBasketRepository basketRepository,
			DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint)
		{
			_discountGrpcService = discountGrpcService;
			_basketRepository = basketRepository;
			_mapper = mapper;
			_publishEndpoint = publishEndpoint;	
		}
		#endregion

		[HttpGet]
		public async Task<IActionResult> GetBasket(string userName)
		{
			var res =await _basketRepository.GetBasketasync(userName);
			return Ok(res);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateBasket(ShoppingCart shoppingCart)
		{
			foreach (var item in shoppingCart.Items)
			{
				var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
				item.Price -= coupon.Amount;
			}
			return Ok(await _basketRepository.UpdateBasketasync(shoppingCart));
		}
		[HttpDelete]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasketasync(userName);
			return Ok();
		}

		[HttpPost]
		//[ProducesResponseType((int)HttpStatusCode.Accepted)]
		//[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
		{
			// get existing basket with total price
			// create basketCheckoutEvent  --  set totalPrice on basketCheckout eventMessage
			// sent checkout event to rabbitmq
			// remove the basket

			var basket = await _basketRepository.GetBasketasync(basketCheckout.UserName);
			if (basket == null)
			{
				return BadRequest();
			}

			// sent checkout event to rabbitmq
			var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
			eventMessage.TotalPrice = basket.TotalPrice;
			await _publishEndpoint.Publish(eventMessage);


			// remove the basket
			await _basketRepository.DeleteBasketasync(basket.UserName);

			return Accepted();
		}
	}
}
