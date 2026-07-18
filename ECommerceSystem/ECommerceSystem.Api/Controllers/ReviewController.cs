using ECommerceSystem.Application.DTOs.Review;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetReviewsByProduct(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReviewResponse>> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewResponse>> CreateReview([FromBody] CreateReviewRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var review = await _reviewService.CreateReviewAsync(userId.Value, request);
            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateReview(int id, [FromBody] UpdateReviewRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _reviewService.UpdateReviewAsync(id, userId.Value, request);
            if (!result)
                return Forbid();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _reviewService.DeleteReviewAsync(id, userId.Value);
            if (!result)
                return Forbid();

            return NoContent();
        }

        private Guid? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)
                           ?? User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }
    }
}
