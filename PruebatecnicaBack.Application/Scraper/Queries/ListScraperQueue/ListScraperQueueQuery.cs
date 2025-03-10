using MediatR;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Scraper.Queries.ListScraperQueue;

public record ListScraperQueueQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IRequest<PagedList<ScraperQueue>>;
