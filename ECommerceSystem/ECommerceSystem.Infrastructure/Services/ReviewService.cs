using AutoMapper;
using ECommerceSystem.Application.DTOs.Review;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;

namespace ECommerceSystem.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.FindAllAsync(r => r.ProductId == productId);
            return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse?> GetReviewByIdAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null) return null;

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse?> CreateReviewAsync(Guid userId, CreateReviewRequest request)
        {
            var review = _mapper.Map<Review>(request);
            review.UserId = userId;
            review.CreatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<bool> UpdateReviewAsync(int id, Guid userId, UpdateReviewRequest request)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null || review.UserId != userId) return false;

            _mapper.Map(request, review);
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(int id, Guid userId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null || review.UserId != userId) return false;

            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
