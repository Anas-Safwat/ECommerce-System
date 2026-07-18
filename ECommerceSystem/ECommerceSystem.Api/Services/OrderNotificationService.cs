using ECommerceSystem.Api.Hubs;
using ECommerceSystem.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ECommerceSystem.Api.Services
{
    public class OrderNotificationService : IOrderNotificationService
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderNotificationService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyOrderStatusChangedAsync(Guid userId, int orderId, string newStatus)
        {
            await _hubContext.Clients.Group($"User_{userId}").SendAsync("OrderStatusUpdated", orderId, newStatus);
        }
    }
}
