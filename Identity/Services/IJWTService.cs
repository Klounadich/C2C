using Identity.Commands;
using Identity.Models;

namespace Identity.Services;

public interface IJWTService
{
    public Task<string> CreateTokenAsync(JWTRequestCommand command);
}