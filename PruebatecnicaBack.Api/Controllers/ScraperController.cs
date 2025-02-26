using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebatecnicaBack.Application.Scraper.Commands.GetData;

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
        public async Task<IActionResult> GetData([FromQuery] int year = 2024, [FromQuery] bool update = false)
        {
            var result = await _mediator.Send(new GetDataCommand(year, update));
            return Res("Success", StatusCodes.Status204NoContent, "Datos guardados correctamente.", result);
        }
    }
}
