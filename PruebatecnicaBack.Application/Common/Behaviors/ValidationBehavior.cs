using PruebatecnicaBack.Application.Authentication.Commands.Common;
using PruebatecnicaBack.Application.Authentication.Commands.Register;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace PruebatecnicaBack.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? iValidator = null)
        {
            _validator=iValidator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validator is null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if(validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors.ConvertAll(ValidationFailure => Error.Validation(ValidationFailure.PropertyName,ValidationFailure.ErrorMessage)).ToList();
            return (dynamic)errors;
        }
    }
}
