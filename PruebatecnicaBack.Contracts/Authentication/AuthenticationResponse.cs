using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Contracts.Roles;

namespace PruebatecnicaBack.Contracts.Authentication
{
    public record AuthenticationResponse(
        string Token,
        string RefreshToken,
        DateTime RefreshTokenExpiration,
        DateTime Expiration,
        UserResponse Account,
        List<RoleResponse> Roles);
}
