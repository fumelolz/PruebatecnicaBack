using PruebatecnicaBack.Application.Authentication.Commands.Common;
using PruebatecnicaBack.Application.Common.Responses;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebatecnicaBack.Application.Authentication.Commands.Login
{
    public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;
}
