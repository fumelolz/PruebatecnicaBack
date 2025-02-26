using PruebatecnicaBack.Api.Common.Http;
using PruebatecnicaBack.Application.Common.Responses;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PruebatecnicaBack.Api.Controllers
{
    [Authorize]
    public class ApiController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Problem(List<Error> errors)
        {
            if(errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            return Problem(errors[0]);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Res<T>(string status, int statusCode, string message, T data)
        {
            var response = new ResponseResult<T>(status, statusCode, message, DateTime.UtcNow, data);


            if(statusCode == 204)
            {
                return NoContent();
            }
            
            if(statusCode == 201)
            {
                var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

                var url = location.AbsoluteUri;
                return Created(url, response);
            }

            return Ok(response);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            return Problem(statusCode: statusCode, title: error.Description);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelStateDictionary);
        }
    }
}
