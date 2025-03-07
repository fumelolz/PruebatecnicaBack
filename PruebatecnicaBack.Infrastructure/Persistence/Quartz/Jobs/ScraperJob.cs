using Microsoft.AspNetCore.SignalR;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Infrastructure.Persistence.SignalR;
using Quartz;

namespace PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;

public class ScraperJob : IJob
{
    private readonly IScraperService _scraperService;
    private readonly IZoneRepository _zoneRepository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public ScraperJob(IScraperService scraperService, IZoneRepository zoneRepository, IHubContext<NotificationHub> hubContext)
    {
        _scraperService=scraperService;
        _zoneRepository=zoneRepository;
        _hubContext=hubContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var jobDataMap = context.MergedJobDataMap;

        if (!jobDataMap.ContainsKey("year") || !jobDataMap.ContainsKey("update"))
        {
            return;
        }

        var year = jobDataMap.GetInt("year");
        var update = jobDataMap.GetBoolean("update");
        var userId = jobDataMap.GetInt("userId");


        Console.WriteLine($"Ejecutando ScraperJob para el año {year}, Update: {update}");
        await Task.Delay(20_000);
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
}
