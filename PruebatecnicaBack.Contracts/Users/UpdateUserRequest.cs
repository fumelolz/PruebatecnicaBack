
namespace PruebatecnicaBack.Contracts.Users
{
    public record UpdateUserRequest(
        string Name,
        string FirstSurname,
        string SecondSurname,
        string Email,
        string Password,
        DateTime Birthday,
        List<int> Roles);
}
