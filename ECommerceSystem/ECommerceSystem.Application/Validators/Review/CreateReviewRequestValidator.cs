using ECommerceSystem.Application.DTOs.Review;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Review
{
    public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be a positive number.");

            RuleFor(x => x.Rating)
                .InclusiveBetween((short)1, (short)5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}
