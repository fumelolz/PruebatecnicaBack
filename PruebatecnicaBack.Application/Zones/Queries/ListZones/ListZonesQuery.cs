using MediatR;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Zones.Queries.ListZones;

public record ListZonesQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int? year,
    decimal? MinCapacity,
    decimal? MaxCapacity,
    int Page,
    int PageSize) : IRequest<PagedList<Zone>>;
