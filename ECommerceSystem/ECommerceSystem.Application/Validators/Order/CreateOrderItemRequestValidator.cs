using ECommerceSystem.Application.DTOs.Order;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Order
{
    public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Quantity must be at least 1.");
        }
    }
}
