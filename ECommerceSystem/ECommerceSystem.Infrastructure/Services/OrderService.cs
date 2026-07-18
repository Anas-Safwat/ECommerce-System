using AutoMapper;
using ECommerceSystem.Application.DTOs.Order;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;
using ECommerceSystem.Domain.Enums;

namespace ECommerceSystem.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _unitOfWork.Orders.FindAllAsync(o => o.UserId == userId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id, Guid userId, bool isAdmin = false)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return null;

            if (!isAdmin && order.UserId != userId) return null;

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse?> CreateOrderAsync(Guid userId, CreateOrderRequest request)
        {
            if (request.Items == null || !request.Items.Any())
                return null;

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                OrderDate = DateTimeOffset.UtcNow,
                TotalAmount = 0 // Will calculate
            };

            var orderItems = new List<OrderItem>();
            foreach (var itemReq in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemReq.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = itemReq.Quantity,
                        UnitPrice = product.Price
                    };
                    orderItems.Add(orderItem);
                    order.TotalAmount += product.Price * itemReq.Quantity;

                    // Reduce stock (optional, depending on business rule)
                    // product.Stock -= itemReq.Quantity;
                    // _unitOfWork.Products.Update(product);
                }
            }

            order.OrderItems = orderItems;
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return false;

            order.Status = request.Status;
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
