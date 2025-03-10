using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebatecnicaBack.Application.Scraper.Commands.GetData;
using PruebatecnicaBack.Application.Scraper.Queries.ListScraperQueue;

namespace PruebatecnicaBack.Api.Controllers
{
    [Route("api/v1/scraper")]
    [ApiController]
    [AllowAnonymous]
    public class ScraperController : ApiController
    {
        private readonly IMediator _mediator;

        public ScraperController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [HttpGet()]
        public async Task<IActionResult> GetData([FromQuery] int userId, [FromQuery] int year = 2024, [FromQuery] bool update = false)
        {
            var result = await _mediator.Send(new GetDataCommand(userId, year, update));
            return Res("Success", StatusCodes.Status204NoContent, "Datos guardados correctamente.", result);
        }

        [HttpGet("pendingJobs")]
        public async Task<IActionResult> GetPendingJobs(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new ListScraperQueueQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);
            var result = await _mediator.Send(query);
            return Res("Success", StatusCodes.Status200OK, "Lista de trabajos pendientes.", result);
        }
    }
}
