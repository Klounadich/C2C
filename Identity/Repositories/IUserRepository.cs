using Identity.DTO;
using Identity.Models;

namespace Identity.Repositories;

public interface IUserRepository
{
    public Task<bool> EmailExistsAsync(string email);
    public Task<bool> UserNameExistsAsync(string email);
    public Task<bool> RegisterAsync(User user);
    public Task<User> GetUserByEmailAsync(string email);
    public Task<User> GetUserByIdAsync(string userId);
    
    public Task<bool>UpdateRefreshTokenAsync(Guid userId, string refreshToken , Guid familyId , DateTime ExpiresAt);
    
    public Task<RefreshTokens> GetRefreshTokenAsync(string refreshToken_hashed);
    public Task<User> EmailConfirmedAsync(Guid userId);
    public Task<bool>RevokeTokenAsync(RefreshTokens token);
    public Task<bool>VerificateEmailAsync(Guid CodeId, string code_hash);
    public Task<string> SaveVerificationCodeAsync(string code_hash , Guid CodeId);
    public Task<bool> Enable2FA(User user);
}