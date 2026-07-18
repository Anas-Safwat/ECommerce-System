using ECommerceSystem.Application.DTOs.Cart;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Cart
{
    public class AddCartItemRequestValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.");
        }
    }
}
