using ECommerceSystem.Application.DTOs.Product;
using ECommerceSystem.Application.DTOs.Common;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductResponse>> GetAllProductsAsync(ProductQueryParameters queryParams);
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<ProductResponse?> CreateProductAsync(Guid sellerId, CreateProductRequest request);
        Task<bool> UpdateProductAsync(int id, Guid sellerId, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id, Guid sellerId);
    }
}
