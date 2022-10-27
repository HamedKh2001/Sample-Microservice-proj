﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System.Net;

namespace Ordering.API.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		#region DI
		private readonly IMediator _mediator;
		#endregion

		#region Ctor
		public OrderController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}
		#endregion

		[HttpGet("{userName}", Name = "GetOrder")]
		[ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
		{
			var query = new GetOrdersListQuery(userName);
			var orders = await _mediator.Send(query);
			return Ok(orders);
		}


		//Testing Purpose
		[HttpPost(Name = "CheckoutOrder")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpPut(Name = "UpdateOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
		{
			await _mediator.Send(command);
			return NoContent();
		}


		[HttpDelete("{id}", Name = "DeleteOrder")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteOrder(int id)
		{
			var command = new DeleteOrderCommand() { Id = id };
			await _mediator.Send(command);

			return NoContent();
		}

	}
}