using Identity.DTO;
using Identity.Models;

namespace Identity.Repositories;

public interface IUserRepository
{
    public Task<bool> EmailExistsAsync(string email);
    public Task<bool> UserNameExistsAsync(string email);
    public Task<bool> RegisterAsync(User user);
    public Task<User> GetUserByEmailAsync(string email);
    
    public Task<bool>UpdateRefreshTokenAsync(Guid userId, string refreshToken , Guid familyId , DateTime ExpiresAt);
}