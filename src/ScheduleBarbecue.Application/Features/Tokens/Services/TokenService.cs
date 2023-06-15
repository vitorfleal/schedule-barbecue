using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScheduleBarbecue.Application.Features.Tokens.Services.Contracts;
using ScheduleBarbecue.Application.Features.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScheduleBarbecue.Application.Features.Tokens.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var claimsToAdd = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("uid", user.Id.ToString())
            };

        var daysToExpire = int.Parse(_configuration["JWT:DaysToExpire"]);

        var token = PrepareToken(claimsToAdd, daysToExpire);

        return token;
    }

    protected string PrepareToken(IList<Claim> claims, int daysToExpire)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = null,
            Issuer = null,
            Expires = DateTime.UtcNow.AddDays(daysToExpire),
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}