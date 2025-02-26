using PruebatecnicaBack.Application.Common.Responses;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Interfaces.scrapping;

public interface IScraperService
{
    Task<List<Zone>> GetData(int year);
}
