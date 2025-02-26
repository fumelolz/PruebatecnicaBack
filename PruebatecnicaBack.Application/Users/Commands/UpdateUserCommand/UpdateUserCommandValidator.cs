using FluentValidation;

namespace PruebatecnicaBack.Application.Users.Commands.UpdateUsercommand
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.FirstSurname).NotEmpty();
            RuleFor(x => x.SecondSurname).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty();
        }
    }
}
