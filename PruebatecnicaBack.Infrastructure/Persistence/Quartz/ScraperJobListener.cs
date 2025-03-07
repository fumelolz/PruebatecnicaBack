using Quartz;

namespace PruebatecnicaBack.Infrastructure.Persistence.Quartz;

public class ScraperJobListener : IJobListener
{
    public string Name => "ScraperJobListener";

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Iniciando trabajo: {context.JobDetail.Key}");
        return Task.CompletedTask;
    }

    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Finalizó trabajo: {context.JobDetail.Key}");

        if (jobException != null)
        {
            Console.WriteLine($"Error en el trabajo: {jobException.Message}");
        }

        return Task.CompletedTask;
    }
}
