using PruebatecnicaBack.Application.Authentication.Commands.Login;
using PruebatecnicaBack.Application.Users.Commands.Register;
using PruebatecnicaBack.Contracts.Authentication;
using PruebatecnicaBack.Contracts.Users;
using Mapster;

namespace PruebatecnicaBack.Api.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateUserRequest, CreateUserCommand>();
            config.NewConfig<LoginRequest, LoginQuery>();
        }
    }
}
