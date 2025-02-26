using MediatR;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Application.Scraper.Commands.GetData;

namespace PruebatecnicaBack.Application.Scraper.Queries.GetProductData;

public class GetDataCommandHandler : IRequestHandler<GetDataCommand, string>
{
    private readonly IScraperService _scraperService;
    private readonly IZoneRepository _zoneRepository;

    public GetDataCommandHandler(IScraperService scraperService, IZoneRepository zoneRepository)
    {
        _scraperService=scraperService;
        _zoneRepository=zoneRepository;
    }

    public async Task<string> Handle(GetDataCommand request, CancellationToken cancellationToken)
    {
        if (request.Update)
        {
            await _zoneRepository.DeleteByYearAsync(request.Year);
        }

        var zones = await _scraperService.GetData(request.Year);
        try
        {
            await _zoneRepository.BulkInsertAsync(zones);
        }catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return "Ok";
    }
}
