﻿using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PruebatecnicaBack.Application.Common.Interfaces.Scraper;

namespace PruebatecnicaBack.Infrastructure.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {

        services.AddScoped<IScraperProducer, ScraperProducer>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ScraperConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("104.197.121.15", "/", h =>
                {
                    h.Username("admin");
                    h.Password("admin");
                });

                // Configura el consumidor para la cola 'scraper_queue'
                cfg.ReceiveEndpoint("scraper_queue", e =>
                {
                    e.UseRawJsonDeserializer(isDefault: true);
                    e.ConfigureConsumer<ScraperConsumer>(context);
                    e.UseConcurrencyLimit(1);
                });
            });
        });

        services.AddMassTransitHostedService();

        return services;
    }
}
