namespace ECommerceSystem.Domain.Entities
{
    public class ProductCategory //Join Table
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

    }
}
