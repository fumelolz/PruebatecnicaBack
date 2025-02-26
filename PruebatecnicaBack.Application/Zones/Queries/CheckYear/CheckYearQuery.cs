using MediatR;

namespace PruebatecnicaBack.Application.Zones.Queries.CheckYear;

public record CheckYearQuery(int Year) : IRequest<bool>;
