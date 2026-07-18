using AutoMapper;
using ECommerceSystem.Application.DTOs.Order;
using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;
using ECommerceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderNotificationService _notificationService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IOrderNotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id, Guid userId, bool isAdmin = false)
        {
            var order = await _unitOfWork.Orders.GetQueryable()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
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
                TotalAmount = 0
            };

            var orderItems = new List<OrderItem>();
            foreach (var itemReq in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemReq.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {itemReq.ProductId} was not found.");

                if (product.Stock < itemReq.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}, Requested: {itemReq.Quantity}.");

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemReq.Quantity,
                    UnitPrice = product.Price
                };
                orderItems.Add(orderItem);
                order.TotalAmount += product.Price * itemReq.Quantity;

                // Reduce stock
                product.Stock -= itemReq.Quantity;
                _unitOfWork.Products.Update(product);
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

            await _notificationService.NotifyOrderStatusChangedAsync(order.UserId, order.Id, order.Status.ToString());

            return true;
        }
    }
}
