using Dapper;
using Discount.API.Entities;
using Npgsql;
using System.Data;

namespace Discount.API.Repository
{
	public class DiscountRepository : IDiscountRepository
	{
		#region Dependency Injection
		private readonly IConfiguration _configuration;
		#endregion

		#region Properties
		//private readonly IDbConnection _dbConnection;
		#endregion

		#region Ctor
		public DiscountRepository(IConfiguration configuration)
		{
			//_dbConnection = new NpgsqlConnection
			//	(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			_configuration = configuration;
		}
		#endregion

		#region IDiscountRepository
		public async Task<bool> CreateDiscountasync(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			try
			{
				var res = await connection
				.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", 
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
				if (res > 0)
					return true;
			}
			catch (Exception ex)
			{

				throw;
			}
			return false;
		}

		public async Task<bool> DeleteDiscountasync(string productName)
		{
			using var connection = new NpgsqlConnection
					(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var res = await connection
				.ExecuteAsync("Delete From Coupon Where ProductName = @ProductName",
				new { ProductName = productName });
			if (res > 0)
				return true;
			return false;
		}

		public async Task<Coupon> GetDiscountasync(string productName)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var coupon = await connection
				.QueryFirstOrDefaultAsync<Coupon>("select * from coupon where ProductName = @productName", new { ProductName = productName });
			return coupon;
		}

		public async Task<bool> UpdateDiscountasync(Coupon coupon)
		{
			using var connection = new NpgsqlConnection
				(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			var res = await connection
				.ExecuteAsync("Update Coupon Set ProductName=@ProductName,Description=@Description,Amount=@Amount Where Id = @Id",
				new { Id = coupon.Id, ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
			if (res > 0)
				return true;
			return false;
		}
		#endregion
	}
}
