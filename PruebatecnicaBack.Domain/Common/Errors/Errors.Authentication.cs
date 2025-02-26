using ErrorOr;

namespace PruebatecnicaBack.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {
            public static Error InvalidCredentials => Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "Credenciales invalidas");
            public static Error UnAuthorize => Error.Unauthorized(
                code: "Auth.UnAuthorize",
                description: "El token esta caducado");
        }
    }
}
