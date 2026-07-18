using AutoMapper;
using ECommerceSystem.Application.DTOs.User;
using ECommerceSystem.Application.Interfaces;

namespace ECommerceSystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
