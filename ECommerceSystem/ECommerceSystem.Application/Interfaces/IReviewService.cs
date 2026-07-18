using ECommerceSystem.Application.DTOs.Review;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetReviewsByProductIdAsync(int productId);
        Task<ReviewResponse?> GetReviewByIdAsync(int id);
        Task<ReviewResponse?> CreateReviewAsync(Guid userId, CreateReviewRequest request);
        Task<bool> UpdateReviewAsync(int id, Guid userId, UpdateReviewRequest request);
        Task<bool> DeleteReviewAsync(int id, Guid userId);
    }
}
