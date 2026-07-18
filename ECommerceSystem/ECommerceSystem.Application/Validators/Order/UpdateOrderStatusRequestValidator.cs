using ECommerceSystem.Application.DTOs.Order;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Order
{
    public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("A valid order status is required.");
        }
    }
}
