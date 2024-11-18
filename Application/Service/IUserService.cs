using practices.Model;

namespace practices.Service
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
    }
}
