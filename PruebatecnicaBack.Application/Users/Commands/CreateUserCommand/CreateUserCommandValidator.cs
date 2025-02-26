using FluentValidation;

namespace PruebatecnicaBack.Application.Users.Commands.Register
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.FirstSurname).NotEmpty();
            RuleFor(x => x.SecondSurname).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty();
            RuleFor(x => x.Roles).NotEmpty();
        }
    }
}
