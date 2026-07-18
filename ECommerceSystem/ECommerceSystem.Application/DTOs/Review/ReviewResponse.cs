namespace ECommerceSystem.Application.DTOs.Review
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public short Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
