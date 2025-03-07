namespace PruebatecnicaBack.Application.Common.Interfaces.Scraper;

public interface IScraperProducer
{
    Task PublishScraperRequest(int year, bool update, int userId);
}
