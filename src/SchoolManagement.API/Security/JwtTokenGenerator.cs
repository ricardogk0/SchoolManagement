using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.API.Security;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAt) GenerateToken(IEnumerable<Claim> claims);
}

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _settings;
    private readonly SigningCredentials _signingCredentials;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public (string token, DateTime expiresAt) GenerateToken(IEnumerable<Claim> claims)
    {
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresMinutes);

        var jwtToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: _signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (token, expires);
    }
}