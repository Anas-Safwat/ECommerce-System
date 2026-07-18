using ECommerceSystem.Application.DTOs.Cart;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Api.Controllers
{
    [Authorize]
    public class CartController : BaseApiController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemResponse>>> GetCart()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var cartItems = await _cartService.GetCartItemsAsync(userId.Value);
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<ActionResult<CartItemResponse>> AddToCart([FromBody] AddCartItemRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var cartItem = await _cartService.AddCartItemAsync(userId.Value, request);
            if (cartItem == null) return BadRequest();

            return Ok(cartItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(int id, [FromBody] UpdateCartItemRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _cartService.UpdateCartItemAsync(id, userId.Value, request);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveCartItem(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _cartService.RemoveCartItemAsync(id, userId.Value);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            await _cartService.ClearCartAsync(userId.Value);
            return NoContent();
        }
    }
}
