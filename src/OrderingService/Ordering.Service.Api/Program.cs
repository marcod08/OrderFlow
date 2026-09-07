using BuildingBlocks.Api.Middleware;
using BuildingBlocks.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Service.Application.Interfaces;
using Ordering.Service.Application.Orders.CreateOrder;
using Ordering.Service.Infrastructure.Persistence;
using Ordering.Service.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<OrderingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrderingDb")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());


builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();