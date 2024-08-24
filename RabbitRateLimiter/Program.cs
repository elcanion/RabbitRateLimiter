using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitRateLimiter.Consumer;
using RabbitRateLimiter.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        
        cfg.UseFilter(new RateLimitMiddleware(10, TimeSpan.FromMinutes(1)));
    });

    x.AddConsumer<MessageProcessor>();
});

builder.Services.Configure<MassTransitHostOptions>(options => 
{ 
    options.WaitUntilStarted = true; 
    options.StartTimeout = 
    TimeSpan.FromSeconds(30); 
    options.StopTimeout = 
    TimeSpan.FromMinutes(1); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
