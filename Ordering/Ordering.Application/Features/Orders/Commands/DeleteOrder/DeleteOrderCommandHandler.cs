using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exeptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
	public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
	{
		#region Properties
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<DeleteOrderCommandHandler> _logger;
		#endregion

		#region Ctor
		public DeleteOrderCommandHandler(IOrderRepository orderRepository,
										 IMapper mapper,
										 ILogger<DeleteOrderCommandHandler> logger)
		{
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		#endregion

		#region IRequestHandler<DeleteOrderCommand>
		public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			var orderToBeDeleted = await _orderRepository.GetByIdAsync(request.Id);
			if (orderToBeDeleted == null)
			{
				throw new NotFoundException(nameof(Order), request.Id);
			}

			await _orderRepository.DeleteAsync(orderToBeDeleted);
			_logger.LogInformation($"Order {orderToBeDeleted.Id} is successfully deleted.");

			return Unit.Value;
		}
		#endregion
	}
}
