using ECommerceSystem.Application.DTOs.Product;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<ProductResponse?> CreateProductAsync(Guid sellerId, CreateProductRequest request);
        Task<bool> UpdateProductAsync(int id, Guid sellerId, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id, Guid sellerId);
    }
}
