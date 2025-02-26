namespace PruebatecnicaBack.Application.Authentication.Commands.Common
{
    public record AuthenticationResult(
        string Token,
        string RefreshToken,
        DateTime RefreshTokenExpiration,
        DateTime Expiration,
        UserResult Account,
        List<RoleResult> Roles);
}
