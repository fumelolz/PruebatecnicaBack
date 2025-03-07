using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PruebatecnicaBack.Application.Common.Interfaces.Scheduling;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Infrastructure.Persistence.SignalR;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PruebatecnicaBack.Infrastructure.Messaging;

public class ScraperConsumer : IConsumer<ScraperRequest>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IScraperService _scraperService;
    private readonly IZoneRepository _zoneRepository;
    public ScraperConsumer(IHubContext<NotificationHub> hubContext, IScraperService scraperService, IZoneRepository zoneRepository)
    {
        _hubContext=hubContext;
        _scraperService=scraperService;
        _zoneRepository=zoneRepository;
    }

    public async Task Consume(ConsumeContext<ScraperRequest> context)
    {
        await Task.CompletedTask;
        var message = context.Message;
        try
        {
            var year = message.Year;
            var update = message.Update;
            var userId = message.UserId;
            Console.WriteLine($"Escuchando el mensaje {message}");

            Console.WriteLine($"Ejecutando ScraperJob para el año {year}, Update: {update}");
            await Task.Delay(5_000);
            if (update)
            {
                await _zoneRepository.DeleteByYearAsync(year);
            }

            var zones = await _scraperService.GetData(year);
            await _zoneRepository.BulkInsertAsync(zones);



            Console.WriteLine($"Scraping finalizado para el año {year}");
            var connectionId = NotificationHub.GetConnectionId(userId);
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
