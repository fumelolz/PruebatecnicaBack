using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PruebatecnicaBack.Application.Common.Interfaces.Scheduling;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;
using PruebatecnicaBack.Infrastructure.Persistence.SignalR;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PruebatecnicaBack.Infrastructure.Messaging;

public class ScraperConsumer : IConsumer<ScraperRequest>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IScraperService _scraperService;
    private readonly IZoneRepository _zoneRepository;
    private readonly IScraperQueueRepository _scraperQueueRepository;
    public ScraperConsumer(IHubContext<NotificationHub> hubContext, IScraperService scraperService, IZoneRepository zoneRepository, IScraperQueueRepository scraperQueueRepository)
    {
        _hubContext=hubContext;
        _scraperService=scraperService;
        _zoneRepository=zoneRepository;
        _scraperQueueRepository=scraperQueueRepository;
    }

    public async Task Consume(ConsumeContext<ScraperRequest> context)
    {
        await Task.CompletedTask;
        var message = context.Message;
        var year = message.Year;
        var update = message.Update;
        var userId = message.UserId;
        var connectionId = NotificationHub.GetConnectionId(userId);

        try
        {
            
            Console.WriteLine($"Escuchando el mensaje {message}");

            Console.WriteLine($"Ejecutando ScraperJob para el año {year}, Update: {update}");
            await Task.Delay(10_000);
            if (update)
            {
                await _zoneRepository.DeleteByYearAsync(year);
            }

            var zones = await _scraperService.GetData(year);
            await _zoneRepository.BulkInsertAsync(zones);



            Console.WriteLine($"Scraping finalizado para el año {year}");
            await _scraperQueueRepository.MarkJobAsCompleted(year);

            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("JobCompleted", new { message = $"Scraping finalizado para el año {year}", year });
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error procesando mensaje: {ex.Message}");
        }
    }
}

public record ScraperRequest(int Year, bool Update, int UserId);
