namespace PruebatecnicaBack.Domain.Entities;

public class ScraperQueue
{
    public int ScraperQueueId { get; set; }
    public int Year { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
