using PruebatecnicaBack.Application.Users.Commands.Register;
using PruebatecnicaBack.Application.Users.Commands.UpdateUsercommand;
using PruebatecnicaBack.Application.Users.Queries.GetUserByIdQuery;
using PruebatecnicaBack.Application.Users.Queries.ListUsersQuery;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Contracts.Users;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebatecnicaBack.Api.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UsersController(ISender mediator, IMapper mapper)
        {
            _mediator=mediator;
            _mapper=mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {

            var command = _mapper.Map<CreateUserCommand>(request);

            var authResult = await _mediator.Send(command);

            if (authResult.IsError)
            {
                return Problem(authResult.Errors);
            }

            return Res("Success",StatusCodes.Status201Created,"Usuario creado correctamente",authResult.Value);
        }

        [HttpPut("{userId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, int userId)
        {

            var command = new UpdateUserCommand(userId,
                                                request.Name,
                                                request.FirstSurname,
                                                request.SecondSurname,
                                                request.Email,
                                                request.Password,
                                                request.Birthday,
                                                request.Roles);

            var authResult = await _mediator.Send(command);

            return authResult.Match(
                authResult => NoContent(),
                Problem
                );
        }

        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByAll(
            [FromQuery] string? roleName,
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var command = new ListUsersQuery(
                roleName,
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);
            var result = await _mediator.Send(command);

            var users = _mapper.Map<List<UserResponse>>(result.Items);

            var response = new PagedListResponse<UserResponse>(users,
                                                                  result.Page,
                                                                  result.PageSize,
                                                                  result.TotalCount,
                                                                  result.TotalPages,
                                                                  result.HasNextPage,
                                                                  result.HasNextPage);

            return Res("Success", StatusCodes.Status200OK, "Lista de usuarios obtenida correctamente.", response);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int userId)
        {
            var command = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            var user = _mapper.Map<UserResponse>(result.Value);

            return Res("Success", StatusCodes.Status200OK, "Información del usuario obtenida correctamente.", user);
        }
    }
}
