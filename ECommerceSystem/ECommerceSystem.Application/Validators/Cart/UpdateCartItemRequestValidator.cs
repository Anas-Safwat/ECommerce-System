using ECommerceSystem.Application.DTOs.Cart;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Cart
{
    public class UpdateCartItemRequestValidator : AbstractValidator<UpdateCartItemRequest>
    {
        public UpdateCartItemRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.");
        }
    }
}
