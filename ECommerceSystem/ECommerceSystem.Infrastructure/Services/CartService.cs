using AutoMapper;
using ECommerceSystem.Application.DTOs.Cart;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;

namespace ECommerceSystem.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemResponse>> GetCartItemsAsync(Guid userId)
        {
            var cartItems = await _unitOfWork.CartItems.FindAllAsync(c => c.UserId == userId);
            
            // To properly map ProductName and Price, Product must be loaded.
            // If FindAllAsync doesn't include it, we manually populate it here or rely on lazy loading
            var response = _mapper.Map<IEnumerable<CartItemResponse>>(cartItems);
            return response;
        }

        public async Task<CartItemResponse?> AddCartItemAsync(Guid userId, AddCartItemRequest request)
        {
            var existingItem = await _unitOfWork.CartItems.FindAsync(c => c.UserId == userId && c.ProductId == request.ProductId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                _unitOfWork.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = _mapper.Map<CartItem>(request);
                cartItem.UserId = userId;
                await _unitOfWork.CartItems.AddAsync(cartItem);
                existingItem = cartItem;
            }

            await _unitOfWork.SaveChangesAsync();

            // Fetch with product to map correctly
            var addedItem = await _unitOfWork.CartItems.GetByIdAsync(existingItem.Id);
            return _mapper.Map<CartItemResponse>(addedItem);
        }

        public async Task<bool> UpdateCartItemAsync(int id, Guid userId, UpdateCartItemRequest request)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null || cartItem.UserId != userId) return false;

            _mapper.Map(request, cartItem);
            _unitOfWork.CartItems.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveCartItemAsync(int id, Guid userId)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null || cartItem.UserId != userId) return false;

            _unitOfWork.CartItems.Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ClearCartAsync(Guid userId)
        {
            var cartItems = await _unitOfWork.CartItems.FindAllAsync(c => c.UserId == userId);
            _unitOfWork.CartItems.DeleteRange(cartItems);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
