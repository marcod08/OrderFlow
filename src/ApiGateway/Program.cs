using ApiGateway.Resilience;
using Yarp.ReverseProxy.Forwarder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddSingleton<IForwarderHttpClientFactory, ResilientForwarderHttpClientFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapReverseProxy();

app.Run();