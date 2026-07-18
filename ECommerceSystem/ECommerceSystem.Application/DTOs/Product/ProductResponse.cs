using ECommerceSystem.Application.DTOs.Category;

namespace ECommerceSystem.Application.DTOs.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid SellerId { get; set; }
        public string SellerEmail { get; set; } = null!;
        public List<CategoryResponse> Categories { get; set; } = new();
    }
}
