using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
//builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
//	 .AddJsonFile("ocelot.Local.json", optional: false, reloadOnChange: true)
//	 .AddEnvironmentVariables();
builder.Host
	   .ConfigureAppConfiguration((hostingContext, config) =>
	   {
	   	config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
	   })
	   .ConfigureLogging((hostingContext, loggingBuilder) =>
	   {
	   	loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
	   	loggingBuilder.AddConsole();
	   	loggingBuilder.AddDebug();
	   });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
await app.UseOcelot();
app.Run();
