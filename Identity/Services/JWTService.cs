using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    public async Task<string> CreateTokenAsync(JWTRequestCommand request)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
            new Claim(ClaimTypes.UserData, request.RegistrationDate.ToLongDateString())
                
        };           
        
        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_options.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}