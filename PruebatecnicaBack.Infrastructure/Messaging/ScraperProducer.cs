using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PruebatecnicaBack.Application.Common.Interfaces.Scraper;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Infrastructure.Persistence.SignalR;
using RabbitMQ.Client;
using System.Text;

namespace PruebatecnicaBack.Infrastructure.Messaging;

public class ScraperProducer : IScraperProducer
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IScraperQueueRepository _scraperQueueRepository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public ScraperProducer(IPublishEndpoint publishEndpoint, IScraperQueueRepository scraperQueueRepository, IHubContext<NotificationHub> hubContext)
    {
        _publishEndpoint=publishEndpoint;
        _scraperQueueRepository=scraperQueueRepository;
        _hubContext=hubContext;
    }

    public async Task PublishScraperRequest(int year, bool update, int userId)
    {
        var message = new ScraperRequest(year, update, userId);

        var connectionId = NotificationHub.GetConnectionId(userId);

        var isScrapingInProgress = await _scraperQueueRepository.IsScrapingInProgress(year);

        if (isScrapingInProgress)
        {
            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("JobCompleted", new { message = $"Scraping para el año {year} ya está en progreso. Se ignora la solicitud.", year });
            }
            return;
        }

        await _scraperQueueRepository.AddScraperJobAsync(new ScraperQueue { Year = year, UserId = userId });

        await _publishEndpoint.Publish(message);
    }
}
