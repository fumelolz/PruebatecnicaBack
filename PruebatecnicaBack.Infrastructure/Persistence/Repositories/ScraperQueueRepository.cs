using Microsoft.EntityFrameworkCore;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;
using System.Linq.Expressions;

namespace PruebatecnicaBack.Infrastructure.Persistence.Repositories;

public class ScraperQueueRepository : IScraperQueueRepository
{
    private readonly ApplicationDbContext _context;

    public ScraperQueueRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsScrapingInProgress(int year)
    {
        return await _context.ScraperQueues.AnyAsync(j => j.Year == year && !j.IsCompleted);
    }

    public async Task AddScraperJobAsync(ScraperQueue job)
    {
        _context.ScraperQueues.Add(job);
        await _context.SaveChangesAsync();
    }

    public async Task MarkJobAsCompleted(int year)
    {
        var job = await _context.ScraperQueues.FirstOrDefaultAsync(j => j.Year == year && !j.IsCompleted);
        if (job != null)
        {
            job.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PagedList<ScraperQueue>> GetPendingJobs(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize)
    {
        IQueryable<ScraperQueue> ScraperQueueQuery = _context.ScraperQueues;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            ScraperQueueQuery = ScraperQueueQuery.Where(p =>
            p.Year.ToString().Contains(searchTerm) ||
            p.UserId.ToString().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            Expression<Func<ScraperQueue, object>> keySelector = sortColumn.ToLower() switch
            {
                "userId" => scraperQueue => scraperQueue.UserId,
                "year" => scraperQueue => scraperQueue.Year,
                "updatedDate" => scraperQueue => scraperQueue.UpdatedDate,
                "creationDate" => scraperQueue => scraperQueue.CreationDate,
                _ => scraperQueue => scraperQueue.ScraperQueueId
            };

            if (sortOrder.ToLower() == "desc")
            {
                ScraperQueueQuery = ScraperQueueQuery.OrderByDescending(keySelector);
            }
            else
            {
                ScraperQueueQuery = ScraperQueueQuery.OrderBy(keySelector);
            }
        }

        var query = ScraperQueueQuery.Where(j => !j.IsCompleted).AsNoTracking();

        var jobs = await PagedList<ScraperQueue>.CreateAsync(query, page, pageSize);

        return jobs;

        //return await _context.ScraperQueues.Where(j => !j.IsCompleted).ToListAsync();
    }
}
