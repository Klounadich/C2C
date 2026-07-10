using Identity.DTO;
using Identity.Models;

namespace Identity.Repositories;

public interface IUserRepository
{
    public Task<bool> EmailExistsAsync(string email);
    public Task<bool> UserNameExistsAsync(string email);
    public Task<UserResponse> RegisterAsync(User user);
}