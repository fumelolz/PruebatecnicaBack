using MediatR;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Zones.Queries.ListZones;

public class ListZonesQueryHandler : IRequestHandler<ListZonesQuery, PagedList<Zone>>
{
    private readonly IZoneRepository _zoneRepository;

    public ListZonesQueryHandler(IZoneRepository zoneRepository)
    {
        _zoneRepository = zoneRepository;
    }

    public Task<PagedList<Zone>> Handle(ListZonesQuery request, CancellationToken cancellationToken)
    {
        return _zoneRepository.GetAllAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.year,
                request.MinCapacity,
                request.MaxCapacity,
                request.Page,
                request.PageSize);
    }
}
