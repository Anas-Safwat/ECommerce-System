namespace ECommerceSystem.Application.DTOs.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}
