using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebatecnicaBack.Application.Zones.Queries.CheckYear;
using PruebatecnicaBack.Application.Zones.Queries.ListZones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PruebatecnicaBack.Api.Controllers
{
    [Route("api/v1/zones")]
    [ApiController]
    public class ZonesController : ApiController
    {
        private readonly ISender _mediator;

        public ZonesController(ISender mediator)
        {
            _mediator=mediator;
        }

        // GET: api/<ZonesController>
        [HttpGet]
        public async Task<IActionResult> getAll(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int? year,
            [FromQuery] decimal minCapacity = 0,
            [FromQuery] decimal maxCapacity = 1000000,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var command = new ListZonesQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                year,
                minCapacity,
                maxCapacity,
                page,
                pageSize);
            var result = await _mediator.Send(command);

            return Res("Success", StatusCodes.Status200OK, "Lista de usuarios obtenida correctamente.", result);
        }

        // GET api/<ZonesController>/5
        [HttpGet("checkYear")]
        public async Task<IActionResult> Get([FromQuery] int year)
        {
            var query = new CheckYearQuery(year);

            var result = await _mediator.Send(query);

            return Res("Success", StatusCodes.Status200OK, $"Verificación si existen registros con el año: {year}.", result);
        }
    }
}
