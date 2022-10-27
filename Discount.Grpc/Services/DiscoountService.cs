using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;

namespace Discount.Grpc
{
	public class DiscoountService : DiscountProtoService.DiscountProtoServiceBase
	{
		#region Properties
		private readonly IDiscountRepository _repository;
		private readonly ILogger<DiscoountService> _logger;
		private readonly IMapper _mapper;
		#endregion

		#region Ctor
		public DiscoountService(IDiscountRepository repository, ILogger<DiscoountService> logger,
			IMapper mapper)
		{
			_mapper = mapper;
			_repository = repository;
			_logger = logger;
		}
		#endregion


		#region DiscountProtoServiceBase
		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await _repository.GetDiscountasync(request.ProductName);
			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound, $"Discount With ProductName {request.ProductName}"));
			_logger.LogInformation($"Discount retrived ProductName: {coupon.ProductName} , Amount: {coupon.Amount}");
			var couponModel = _mapper.Map<CouponModel>(coupon);
			return couponModel;
		}
		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);
			var res = await _repository.CreateDiscountasync(coupon);
			_logger.LogInformation($"Discount is Successfully Created. ProductName: {coupon.ProductName}");
			var couponModel = _mapper.Map<CouponModel>(request.Coupon);
			return couponModel;
		}
		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon = _mapper.Map<Coupon>(request.Coupon);
			var res = await _repository.UpdateDiscountasync(coupon);
			_logger.LogInformation($"Discount is Successfully Updated. ProductName: {coupon.ProductName}");
			var couponModel = _mapper.Map<CouponModel>(request.Coupon);
			return couponModel;
		}
		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var isSuccessfully = await _repository.DeleteDiscountasync(request.ProductName);
			var res = new DeleteDiscountResponse()
			{
				Success = isSuccessfully
			};
			return res;
		}
		#endregion
	}
}
