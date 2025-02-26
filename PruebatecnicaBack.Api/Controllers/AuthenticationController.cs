using PruebatecnicaBack.Application.Authentication.Commands.Common;
using PruebatecnicaBack.Application.Authentication.Commands.Login;
using PruebatecnicaBack.Application.Authentication.Commands.RefreshToken;
using PruebatecnicaBack.Application.Common.Responses;
using PruebatecnicaBack.Application.Users.Commands.Register;
using PruebatecnicaBack.Contracts.Authentication;
using PruebatecnicaBack.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PruebatecnicaBack.Api.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator=mediator;
            _mapper=mapper;
        }

        

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResult = await _mediator.Send(query);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }

            if (authResult.IsError)
            {
                return Problem(authResult.Errors);
            }

            return Res("Succes", StatusCodes.Status200OK, "Logeado correctamente", authResult.Value);

        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var query = new RefreshTokenQuery(request.Token);
            var authResult = await _mediator.Send(query);

            return authResult.Match(
                authResult => Res("Succes", StatusCodes.Status200OK, "Logeado correctamente", _mapper.Map<AuthenticationResult>(authResult)),
                errors => Problem(errors)
                );
        }
    }
}
