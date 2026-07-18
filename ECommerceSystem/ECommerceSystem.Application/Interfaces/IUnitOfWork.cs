using ECommerceSystem.Domain.Entities;

namespace ECommerceSystem.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IRepository<User,Guid> Users { get; }
        IRepository<Review,int> Reviews { get; }
        IRepository<Product, int> Products { get; }
        IRepository<OrderItem, int> OrderItems { get; }
        IRepository<Order, int> Orders { get; }
        IRepository<Category, int> Categories { get; }
        IRepository<CartItem, int> CartItems { get; }
        IRepository<RefreshToken, Guid> RefreshTokens { get; }

        Task<int> SaveChangesAsync();
    }
}
