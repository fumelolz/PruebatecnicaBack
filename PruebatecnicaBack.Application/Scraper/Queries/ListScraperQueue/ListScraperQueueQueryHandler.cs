using MediatR;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Scraper.Queries.ListScraperQueue;

public class ListScraperQueueQueryHandler : IRequestHandler<ListScraperQueueQuery, PagedList<ScraperQueue>>
{
    private readonly IScraperQueueRepository _scraperQueueRepository;

    public ListScraperQueueQueryHandler(IScraperQueueRepository scraperQueueRepository)
    {
        _scraperQueueRepository=scraperQueueRepository;
    }

    public Task<PagedList<ScraperQueue>> Handle(ListScraperQueueQuery request, CancellationToken cancellationToken)
    {
        return _scraperQueueRepository.GetPendingJobs(
            request.SearchTerm,
            request.SortColumn,
            request.SortOrder,
            request.Page,
            request.PageSize);
    }
}
