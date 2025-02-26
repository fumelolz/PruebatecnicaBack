using PruebatecnicaBack.Application.Roles.Commands.Common;
using PruebatecnicaBack.Contracts.Roles;
using PruebatecnicaBack.Domain.Entities;
using Mapster;
using System.Globalization;

namespace PruebatecnicaBack.Api.Common.Mapping
{
    public class RoleMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            var formatInfo = new CultureInfo("es-MX").DateTimeFormat;
            formatInfo.DateSeparator = "-";

            config.NewConfig<Role, RoleSimplifiedResponse>();
            config.NewConfig<Role, RoleResponse>();
            //.Map(dest => dest.CreationDate, src => src.CreationDate.ToString("d", formatInfo))
            //.Map(dest => dest.UpdatedDate, src => src.UpdatedDate.ToString("d", formatInfo));

            config.NewConfig<Role, RoleResult>();
        }
    }
}
