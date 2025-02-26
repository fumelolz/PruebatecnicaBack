using ErrorOr;

namespace PruebatecnicaBack.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Role
        {
            public static Error NotFound => Error.NotFound(
                code: "Role.NotFound",
                description: "Role no encontrado.");

            public static Error AlReadyExist => Error.Conflict(
                code: "Role.AlReadyExist",
                description: "Role ya se encuentra registrado.");
        }
    }
}
