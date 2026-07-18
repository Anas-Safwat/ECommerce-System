using ECommerceSystem.Application.DTOs.Auth;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Auth
{
    public class LogoutRequestValidator : AbstractValidator<LogoutRequest>
    {
        public LogoutRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
