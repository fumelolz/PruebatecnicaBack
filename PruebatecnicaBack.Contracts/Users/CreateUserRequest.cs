namespace PruebatecnicaBack.Contracts.Users
{
    public record CreateUserRequest(
        string Name,
        string FirstSurname,
        string SecondSurname,
        string Email,
        string Password,
        DateTime Birthday,
        List<int> Roles);
}
