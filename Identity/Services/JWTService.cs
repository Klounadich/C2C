using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services;

public class JWTService : IJWTService
{
    public class AuthSettings
    {
        public TimeSpan Expires { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    
    private readonly AuthSettings _options;

  
    public JWTService(IOptions<AuthSettings> options)
    {
        _options = options.Value;
    }

    public async Task<string> CreateRefreshTokenAsync()
    {
        var bytes = new byte[32]; 
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        var token = Convert.ToBase64String(bytes);
        
        return  token;
    }

    public async Task<string> CreateTokenAsync(JWTRequestCommand request)
    { 
       
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
            new Claim(ClaimTypes.UserData, request.RegistrationDate.ToLongDateString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(_options.Expires),
            signingCredentials: credentials
        );
        
        var test = new JwtSecurityTokenHandler().WriteToken(token);
        
        return test;
    }
}