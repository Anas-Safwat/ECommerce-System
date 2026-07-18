using AutoMapper;
using ECommerceSystem.Application.DTOs.Common;
using ECommerceSystem.Application.DTOs.Product;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ECommerceSystem.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        private string GetProductCacheVersion()
        {
            if (!_cache.TryGetValue("ProductCacheVersion", out string? version))
            {
                version = Guid.NewGuid().ToString();
                _cache.Set("ProductCacheVersion", version);
            }
            return version!;
        }

        private void InvalidateProductCache()
        {
            _cache.Set("ProductCacheVersion", Guid.NewGuid().ToString());
        }

        public async Task<PagedResult<ProductResponse>> GetAllProductsAsync(ProductQueryParameters queryParams)
        {
            var cacheVersion = GetProductCacheVersion();
            var cacheKey = $"Products_{cacheVersion}_{JsonSerializer.Serialize(queryParams)}";

            if (_cache.TryGetValue(cacheKey, out PagedResult<ProductResponse>? cachedResult))
            {
                return cachedResult!;
            }

            IQueryable<Product> query = _unitOfWork.Products.GetQueryable()
                .Include(p => p.Seller)
                .Include(p => p.Categories)
                .Include(p => p.Reviews)
                .Include(p => p.OrderItems);

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(queryParams.SearchTerm) || p.Description.Contains(queryParams.SearchTerm));
            }

            if (queryParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == queryParams.CategoryId.Value));
            }

            if (queryParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
            }

            if (queryParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
            }

            if (queryParams.MinRating.HasValue)
            {
                query = query.Where(p => p.Reviews.Any() && p.Reviews.Average(r => r.Rating) >= queryParams.MinRating.Value);
            }

            switch (queryParams.SortBy?.ToLower())
            {
                case "price":
                    query = queryParams.IsDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "rating":
                    query = queryParams.IsDescending 
                        ? query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0) 
                        : query.OrderBy(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0);
                    break;
                case "popularity":
                    query = queryParams.IsDescending 
                        ? query.OrderByDescending(p => p.OrderItems.Count) 
                        : query.OrderBy(p => p.OrderItems.Count);
                    break;
                default:
                    query = queryParams.IsDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt);
                    break;
            }

            var totalCount = await query.CountAsync();
            var products = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                                      .Take(queryParams.PageSize)
                                      .ToListAsync();

            var mappedProducts = _mapper.Map<IEnumerable<ProductResponse>>(products);

            var result = new PagedResult<ProductResponse>
            {
                Items = mappedProducts,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(cacheKey, result, cacheOptions);

            return result;
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Seller)
                .Include(p => p.Categories)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return null;

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse?> CreateProductAsync(Guid sellerId, CreateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.SellerId = sellerId;
            product.CreatedAt = DateTimeOffset.UtcNow;

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                var categories = await _unitOfWork.Categories.FindAllAsync(c => request.CategoryIds.Contains(c.Id));
                product.Categories = categories.ToList();
            }

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            InvalidateProductCache();

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<bool> UpdateProductAsync(int id, Guid sellerId, UpdateProductRequest request)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;

            if (product.SellerId != sellerId)
            {
                return false; 
            }

            _mapper.Map(request, product);

            if (request.CategoryIds != null)
            {
                var newCategories = await _unitOfWork.Categories.FindAllAsync(c => request.CategoryIds.Contains(c.Id));
                product.Categories.Clear();
                foreach (var cat in newCategories)
                {
                    product.Categories.Add(cat);
                }
            }

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            InvalidateProductCache();

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

            InvalidateProductCache();

            return true;
        }
    }
}
