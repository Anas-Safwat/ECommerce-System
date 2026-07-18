namespace ECommerceSystem.Application.DTOs.Product
{
    public class ProductQueryParameters
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
        
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
