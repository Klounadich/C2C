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
}