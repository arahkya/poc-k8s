using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Config Serilog
string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{POD_NAME}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";
string logLocation = Environment.GetEnvironmentVariable("LOG_LOCATION") ?? "logs/log-.txt";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentVariable("POD_NAME")
    .WriteTo.File(logLocation, rollingInterval: RollingInterval.Day, outputTemplate: logTemplate)
    .WriteTo.Console(outputTemplate: logTemplate)
    .CreateLogger();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    Log.Information("Forcast Weather Requested");
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    Log.Debug("Forcast Weather Generated => {@forecast}", forecast);
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

Log.Information("Service Application Started");

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
