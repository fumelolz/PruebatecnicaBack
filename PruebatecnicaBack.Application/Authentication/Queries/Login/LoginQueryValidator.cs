using FluentValidation;

namespace PruebatecnicaBack.Application.Authentication.Commands.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator() 
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
