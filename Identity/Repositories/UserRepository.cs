using Identity.DTO;
using Identity.Infrastructure;
using Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDBContext _context;
    public UserRepository( IdentityDBContext context)
    {
        _context=context;
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        if (await _context.Users.AnyAsync(e => e.Email == email))
        {
            return true;
        }
        return false;
        
    }

    public async Task<bool> UserNameExistsAsync(string email)
    {
        if (await _context.Users.AnyAsync(e => e.Email == email))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> RegisterAsync(User user)
    {
        await _context.Users.AddAsync(user);
        if (await _context.SaveChangesAsync()>0)
        {
           return true;
        }

        return false;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
        if (user != null)
        {
            return user;
        }
        return null;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _context.Users.Where(x => x.Id.ToString() == userId).FirstOrDefaultAsync();
        if (user != null)
        {
            return user;
        }
        return null;
    }

    public async Task<bool> UpdateRefreshTokenAsync(Guid userId, string refreshToken ,Guid familyId , DateTime expiresAt)
    {
         await _context.RefreshTokens.AddAsync(new RefreshTokens
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FamilyId = familyId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        });
         return await _context.SaveChangesAsync() > 0;
    }

    public async Task<RefreshTokens> GetRefreshTokenAsync(string refreshToken_hashed)
    {
        return await _context.RefreshTokens.Where(x => x.TokenHash == refreshToken_hashed).FirstOrDefaultAsync();
    }

    public async Task<bool> RevokeTokenAsync(RefreshTokens token)
    {
        var revoke_token = new RefreshTokens
        {
            Id = token.Id,
            UserId = token.UserId,
            FamilyId = token.FamilyId,
            CreatedAt = token.CreatedAt,
            ExpiresAt = token.ExpiresAt,
            TokenHash = token.TokenHash,
            RevokedAt = DateTime.UtcNow,

        };
        _context.RefreshTokens.Update(revoke_token);
        return await _context.SaveChangesAsync() > 0;
    }
}