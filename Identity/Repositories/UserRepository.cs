using System.ComponentModel.DataAnnotations;
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
            ExpiresAt = expiresAt,
            TokenHash = refreshToken
        });
         return await _context.SaveChangesAsync() > 0;
    }

    public async Task<RefreshTokens> GetRefreshTokenAsync(string refreshToken_hashed)
    {
        return await _context.RefreshTokens.Where(x => x.TokenHash == refreshToken_hashed).FirstOrDefaultAsync();
    }

    public async Task<bool> RevokeTokenAsync(RefreshTokens token)
    {
       
        token.RevokedAt = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<string> 
        SaveVerificationCodeAsync(string code_hash, Guid UserId)
    {
        var code = new VerificationNotification
        {
            UserId = UserId,
            Code_hashed = code_hash,
        };
        await _context.VerificationNotifications.AddAsync(code);
        await _context.SaveChangesAsync();
        return code.Id.ToString();

    }

    public async Task<bool> VerificateEmailAsync(Guid CodeId, string code_hash)
    {
        var verification = await _context.VerificationNotifications
            .Where(x => x.Id == CodeId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (verification is null)
            throw new ValidationException("No active verification code. Please request a new one.");

        if (verification.ExpiresAt < DateTime.UtcNow)
            throw new ValidationException("Code expired. Please request a new one.");

        if (verification.Attempts >= 5)
            throw new ValidationException("Too many attempts. Please request a new code.");

        if (verification.Code_hashed != code_hash)
        {
            verification.Attempts++;
            await _context.SaveChangesAsync();
            throw new ValidationException("Invalid confirmation code");
        }

        return true;
    }

    public async Task<User> EmailConfirmedAsync(Guid UserId)
    {
        var user = await _context.Users.Where(x=>x.Id == UserId).FirstOrDefaultAsync();
        user.EmailConfirmed = true;
        await _context.SaveChangesAsync();
        return user;
    }


    public async Task<bool> Enable2FA(User user)
    {
        user.TwoFactorEnabled = true;
        await _context.SaveChangesAsync();
        return true;
    }
}