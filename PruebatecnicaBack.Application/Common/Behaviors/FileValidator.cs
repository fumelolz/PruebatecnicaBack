using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace PruebatecnicaBack.Application.Common.Behaviors
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(6 * 1024 * 1024)
                .WithMessage("El archivo es más pesado que lo validado");

            RuleFor(x => x.ContentType).NotNull().Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("Archivo con formato incorecto");
        }
    }
}
