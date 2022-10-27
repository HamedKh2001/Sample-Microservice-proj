using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient _protoServiceClient;
		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient protoServiceClient)
		{
			_protoServiceClient = protoServiceClient;
		}


		public async Task<CouponModel> GetDiscount(string productName)
		{
			var discountRequest = new GetDiscountRequest
			{
				ProductName = productName,
			};

			var x = await _protoServiceClient.GetDiscountAsync(discountRequest);
			return x;

		}
	}
}
