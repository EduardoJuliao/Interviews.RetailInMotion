using Interviews.RetailInMotion.Domain.Interfaces.Repositories;
using Interviews.RetailInMotion.Domain.Interfaces.Services;
using Interviews.RetailInMotion.Domain.Mappers;
using Interviews.RetailInMotion.Domain.Services;
using Interviews.RetailInMotion.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InterviewDb")
);

services.AddSingleton<IOrderService, OrderService>();
services.AddSingleton<IStockService, StockService>();

services.AddSingleton<IOrderRepository, OrderRepository>();
services.AddSingleton<IStockRepository, StockRepository>();

services.AddAutoMapper(typeof(OrderMapperProfile));

var serilogger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.AddSerilog(serilogger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
