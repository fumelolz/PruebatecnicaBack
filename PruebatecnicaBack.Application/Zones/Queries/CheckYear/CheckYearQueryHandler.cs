using MediatR;
using PruebatecnicaBack.Application.Common.Persistence;

namespace PruebatecnicaBack.Application.Zones.Queries.CheckYear;

public class CheckYearQueryHandler : IRequestHandler<CheckYearQuery, bool>
{
    private readonly IZoneRepository _zoneRepository;

    public CheckYearQueryHandler(IZoneRepository zoneRepository)
    {
        _zoneRepository=zoneRepository;
    }

    public async Task<bool> Handle(CheckYearQuery request, CancellationToken cancellationToken)
    {
        return await _zoneRepository.ExistsByYearAsync(request.Year);
    }
}
