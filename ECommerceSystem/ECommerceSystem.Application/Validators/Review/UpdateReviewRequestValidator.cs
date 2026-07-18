using ECommerceSystem.Application.DTOs.Review;
using FluentValidation;

namespace ECommerceSystem.Application.Validators.Review
{
    public class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequest>
    {
        public UpdateReviewRequestValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween((short)1, (short)5).WithMessage("Rating must be between 1 and 5.")
                .When(x => x.Rating.HasValue);

            RuleFor(x => x.Comment)
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
