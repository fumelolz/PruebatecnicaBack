using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Contracts.Roles;
using PruebatecnicaBack.Domain.Entities;
using Mapster;
using Microsoft.OpenApi.Writers;

namespace PruebatecnicaBack.Api.Common.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserResponse>()
                .Map(dest => dest.Roles, src => GetRoleList(src));

        }

        public List<RoleSimplifiedResponse> GetRoleList(User user)
        {
            
            var resultList = new List<RoleSimplifiedResponse> ();

            foreach (var userRole in user.UserRoles)
            {
                resultList.Add(userRole.Role.Adapt<RoleSimplifiedResponse>());
            }
            return resultList;
        }

    }
}
