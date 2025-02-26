namespace PruebatecnicaBack.Application.Authentication.Commands.Common
{
    public record UserResult(int UserId, string Email, string Name, string FirstSurname, string SecondSurname);
}
