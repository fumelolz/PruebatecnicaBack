using ErrorOr;

namespace PruebatecnicaBack.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(code: "User.DuplicateEmail", description: "El correo electrónico ya está registrado");
            public static Error NotFound => Error.NotFound(code: "User.NotFound", description: "El usuario no está registrado");
        }
    }
}
