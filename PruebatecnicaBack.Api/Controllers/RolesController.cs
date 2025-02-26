using PruebatecnicaBack.Application.Roles.Commands.CreateRole;
using PruebatecnicaBack.Application.Roles.Commands.UpdateRole;
using PruebatecnicaBack.Application.Roles.Queries.ListRoles;
using PruebatecnicaBack.Application.Roles.Queries.RoleById;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Contracts.Roles;
using PruebatecnicaBack.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PruebatecnicaBack.Api.Controllers
{
    [Route("api/v1/auth/roles")]
    [ApiController]
    public class RolesController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public RolesController(ISender mediator, IMapper mapper)
        {
            _mediator=mediator;
            _mapper=mapper;
        }


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetById(int roleId)
        {
            var command = new GetRoleByIdQuery(roleId);
            var result = await _mediator.Send(command);


            if(result.IsError && result.FirstError == Errors.Role.NotFound)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: result.FirstError.Description);
            }

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            var role = _mapper.Map<RoleResponse>(result.Value);

            return Res("Success", StatusCodes.Status200OK, "Role obtenido correctamente.", role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchTerm,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var command = new ListRoleQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize);

            var result = await _mediator.Send(command);

            var roles = _mapper.Map<List<RoleResponse>>(result.Items);
            var response = new PagedListResponse<RoleResponse>(roles,
                                                                  result.Page,
                                                                  result.PageSize,
                                                                  result.TotalCount,
                                                                  result.TotalPages,
                                                                  result.HasNextPage,
                                                                  result.HasNextPage);

            return Res("Success", StatusCodes.Status200OK, "Lista de roles obtenida correctamente.", response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateRoleRequest request)
        {
            var command = new CreateRoleCommand(request.Name,request.Description);
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            var role = _mapper.Map<RoleResponse>(result.Value);

            return Res("Success", StatusCodes.Status201Created, "Rol creado correctamente.", role);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> Update(int roleId, [FromBody] UpdateRoleRequest request)
        {
            var command = new UpdateRoleCommand(roleId, request.Name, request.Description);
            var result = await _mediator.Send(command);

            if (result.IsError && result.FirstError == Errors.Role.NotFound)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: result.FirstError.Description);
            }

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            var role = _mapper.Map<RoleResponse>(result.Value);

            return Res("Success", StatusCodes.Status204NoContent, "Rol modificado correctamente.", role);
        }
    }
}
