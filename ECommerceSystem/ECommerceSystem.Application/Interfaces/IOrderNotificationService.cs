namespace ECommerceSystem.Application.Interfaces
{
    public interface IOrderNotificationService
    {
        Task NotifyOrderStatusChangedAsync(Guid userId, int orderId, string newStatus);
    }
}
