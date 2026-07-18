using ECommerceSystem.Application.DTOs.Product;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Product
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50).WithMessage("Product name cannot exceed 50 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.")
                .When(x => x.Stock.HasValue);

            RuleFor(x => x.CategoryIds)
                .ForEach(id => id.GreaterThan(0).WithMessage("Category ID must be a positive number."))
                .When(x => x.CategoryIds != null && x.CategoryIds.Count > 0);
        }
    }
}
