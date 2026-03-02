using MyDay.Core.Application.Abstractions;
using MyDay.Core.Application.Concrete;
using MyDay.Core.Infrastructure.Abstractions;
using MyDay.Core.Infrastructure.Concrete;
using MyDay.Core.Services.Abstractions;
using MyDay.Core.Services.Concrete;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .Build();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddHttpClient();

//=> HTTP Clients
builder.Services.AddHttpClient("news-api", client =>
{
    client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("NewsAPISettings:Timeout")); 
});
builder.Services.AddHttpClient("open-weather-api", client =>
{
    client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("OpenWeatherAPISettings:Timeout"));
});
builder.Services.AddHttpClient("tidal-api", client =>
{
    client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("TidalAPISettings:Timeout"));
});
builder.Services.AddHttpClient("tidal-api-auth", client =>
{
    client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("TidalAPISettings:Timeout"));
});

//=> Caching
builder.Services.AddMemoryCache(); 
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetValue<string>("CacheSettings:RedisConnectionString");
});

//=> MyDay.Core
builder.Services.AddScoped<ICachingOperations, CachingOperationsService>();
builder.Services.AddScoped<IHttpOperations, HttpOperationsService>();
builder.Services.AddScoped<INewsOperations, NewsAPIOperationsService>();
builder.Services.AddScoped<IWeatherOperations, OpenWeatherAPIOperationsService>();
builder.Services.AddScoped<IMusicOperations, TidalAPIOperationsService>();
builder.Services.AddScoped<IDailyTipsOperations, DailyTipsOperationsService>();
builder.Services.AddScoped<IPerformanceOperations, PerformanceOperationsService>();

//=> Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//=> Build & run the app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Day API");
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
