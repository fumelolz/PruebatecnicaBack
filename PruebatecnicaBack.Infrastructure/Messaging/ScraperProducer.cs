using MassTransit;
using Newtonsoft.Json;
using PruebatecnicaBack.Application.Common.Interfaces.Scraper;
using RabbitMQ.Client;
using System.Text;

namespace PruebatecnicaBack.Infrastructure.Messaging;

public class ScraperProducer : IScraperProducer
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ScraperProducer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint=publishEndpoint;
    }

    public async Task PublishScraperRequest(int year, bool update, int userId)
    {
        var message = new ScraperRequest(year, update, userId);
        await _publishEndpoint.Publish(message);
    }
}
