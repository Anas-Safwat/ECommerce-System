using ECommerceSystem.Application.DTOs.Product;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceSystem.Application.DTOs.Common;

namespace ECommerceSystem.Api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetAllProducts([FromQuery] ProductQueryParameters queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponse>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = "SellerOrAdmin")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var product = await _productService.CreateProductAsync(userId.Value, request);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SellerOrAdmin")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _productService.UpdateProductAsync(id, userId.Value, request);
            if (!result)
                return Forbid();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SellerOrAdmin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null) return Unauthorized();

            var result = await _productService.DeleteProductAsync(id, userId.Value);
            if (!result)
                return Forbid();

            return NoContent();
        }
    }
}
