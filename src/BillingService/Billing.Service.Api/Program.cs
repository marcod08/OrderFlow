using Billing.Service.Application.Consumers;
using Billing.Service.Application.Payments.ProcessPayment;
using Billing.Service.Infrastructure.Persistence;
using Billing.Service.Infrastructure.Repositories;
using Billing.Service.Application.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BillingDb")));

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<ProcessPaymentCommand>());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentRequestedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"] ?? "localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMq:Password"] ?? "guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<BillingDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.Run();