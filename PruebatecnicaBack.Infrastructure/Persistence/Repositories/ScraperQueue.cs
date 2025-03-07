using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;

namespace PruebatecnicaBack.Infrastructure.Persistence.Repositories;

public class ScraperQueue : IScraperQueueRepository
{
    private readonly ApplicationDbContext _context;

    public ScraperQueue(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddScraperJobAsync(Domain.Entities.ScraperQueue job)
    {
        throw new NotImplementedException();
    }

    public Task<List<Domain.Entities.ScraperQueue>> GetPendingJobs()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsScrapingInProgress(int year)
    {
        throw new NotImplementedException();
    }

    public Task MarkJobAsCompleted(int year)
    {
        throw new NotImplementedException();
    }

    //public async Task<bool> IsScrapingInProgress(int year)
    //{
    //    return await _context.ScraperQueues.AnyAsync(j => j.Year == year && !j.IsCompleted);
    //}

    //public async Task AddScraperJobAsync(ScraperQueue job)
    //{
    //    _context.ScraperQueues.Add(job);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task MarkJobAsCompleted(int year)
    //{
    //    var job = await _context.ScraperQueues.FirstOrDefaultAsync(j => j.Year == year && !j.IsCompleted);
    //    if (job != null)
    //    {
    //        job.IsCompleted = true;
    //        await _context.SaveChangesAsync();
    //    }
    //}

    //public async Task<List<ScraperJob>> GetPendingJobs()
    //{
    //    return await _context.ScraperQueues.Where(j => !j.IsCompleted).ToListAsync();
    //}
}
