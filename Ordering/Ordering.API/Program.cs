using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extentions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

#region CAP
builder.Services.AddCap(x =>
{
	x.UseInMemoryMessageQueue();
	x.UseInMemoryStorage();
	//todo: add to config
	x.UseRabbitMQ(builder.Configuration.GetSection("EventBusSettings")["HostAddress"]);
});
#endregion

builder.Services.AddMassTransit(config =>
{
	config.AddConsumer<BasketCheckoutConsumer>();
	config.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
		{
			c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
		});
	});
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
ApplicationServiceRegister.AddApplicationServices(builder.Services);
InfrastructionServiceRegister.AddApplicationServices(builder.Services, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ApplicationServiceRegistration.AddApplicationServices(builder.Services);

var app = builder.Build();
//Seed Context
app.MigrateDatabase<OrderContext>((context, service) =>
{
	var logger = service.GetService<ILogger<OrderContextSeed>>();
	OrderContextSeed
		.SeedAsync(context, logger)
		.Wait();
});

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();