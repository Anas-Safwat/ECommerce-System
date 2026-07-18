using AutoMapper;
using ECommerceSystem.Application.DTOs.Product;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;

namespace ECommerceSystem.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            // Note: EF Core navigation properties might need Include, but IRepository might not support it easily yet.
            // If they are lazy loaded, mapper will work. Otherwise we might have null navigation props.
            return _mapper.Map<IEnumerable<ProductResponse>>(products);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse?> CreateProductAsync(Guid sellerId, CreateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.SellerId = sellerId;
            product.CreatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<bool> UpdateProductAsync(int id, Guid sellerId, UpdateProductRequest request)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            // Optional: enforce seller ownership (if the service layer enforces this instead of controller)
            if (product.SellerId != sellerId)
            {
                // Verify if the user is an admin or we just return false for unauthorized
                return false; 
            }

            _mapper.Map(request, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int id, Guid sellerId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            if (product.SellerId != sellerId)
            {
                return false; 
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
