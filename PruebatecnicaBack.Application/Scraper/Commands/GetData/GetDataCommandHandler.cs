using MediatR;
using PruebatecnicaBack.Application.Common.Interfaces.Scheduling;
using PruebatecnicaBack.Application.Common.Interfaces.Scraper;
using PruebatecnicaBack.Application.Scraper.Commands.GetData;
using Quartz;
namespace PruebatecnicaBack.Application.Scraper.Queries.GetProductData;

public class GetDataCommandHandler : IRequestHandler<GetDataCommand, string>
{
    private readonly IJobScheduler _jobScheduler;
    private readonly IScraperProducer _scraperProducer;

    public GetDataCommandHandler(IJobScheduler jobScheduler, IScraperProducer scraperProducer)
    {
        _jobScheduler=jobScheduler;
        _scraperProducer=scraperProducer;
    }

    public async Task<string> Handle(GetDataCommand request, CancellationToken cancellationToken)
    {
        await _scraperProducer.PublishScraperRequest(request.Year, request.Update, request.UserId);

        //await _jobScheduler.ScheduleScraperJob(request.Year, request.Update, request.UserId, cancellationToken);


        return "ScraperJob programado exitosamente.";
    }
}
