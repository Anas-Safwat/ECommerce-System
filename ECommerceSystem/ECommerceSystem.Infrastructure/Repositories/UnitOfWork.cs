using ECommerceSystem.Application.Interfaces;
using ECommerceSystem.Domain.Entities;
using ECommerceSystem.Infrastructure.Data;
namespace ECommerceSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        private IRepository<User, Guid>? _users;
        private IRepository<Review, int>? _reviews;
        private IRepository<Product, int>? _products;
        private IRepository<OrderItem, int>? _orderItems;
        private IRepository<Order, int>? _orders;
        private IRepository<Category, int>? _categories;
        private IRepository<CartItem, int>? _cartItems;

        // Lazy Initialization
        public IRepository<User, Guid> Users => _users ??= new Repository<User,Guid>(_context);
        public IRepository<Review, int> Reviews => _reviews ??= new Repository<Review, int>(_context);
        public IRepository<Product, int> Products => _products ??= new Repository<Product, int>(_context);
        public IRepository<OrderItem, int> OrderItems => _orderItems ??= new Repository<OrderItem, int>(_context);
        public IRepository<Order, int> Orders => _orders ??= new Repository<Order, int>(_context);
        public IRepository<Category, int> Categories => _categories ??= new Repository<Category, int>(_context);
        public IRepository<CartItem, int> CartItems => _cartItems ??= new Repository<CartItem, int>(_context);
       
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
