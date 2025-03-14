using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using Serilog.Sinks.Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    //.WriteTo.Prometheus(counterNameTemplate: "myapp_{0}", endpoint: "/metrics") // Add Prometheus sink
    .CreateLogger();



builder.Host.UseSerilog(); // Use Serilog for logging

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "This API provides functionalities for managing users and orders."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}


// Enable Prometheus metrics
app.UseMetricServer(url: "/metrics"); // Expose metrics at 
app.UseHttpMetrics(); // Tracks HTTP request metrics

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
