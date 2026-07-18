using ECommerceSystem.Application.DTOs.Order;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetMyOrders()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var orders = await _orderService.GetUserOrdersAsync(userId.Value);
            return Ok(orders);
        }

        [HttpGet("all")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var isAdmin = User.IsInRole("Admin");

            var order = await _orderService.GetOrderByIdAsync(id, userId.Value, isAdmin);
            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var order = await _orderService.CreateOrderAsync(userId.Value, request);
            if (order == null) return BadRequest();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "AdminOnly")] // Can also be SellerOrAdmin
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, request);
            if (!result) return NotFound();

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
