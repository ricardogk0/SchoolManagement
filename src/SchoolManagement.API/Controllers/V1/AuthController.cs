using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Security;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.DTOs.Response;
using SchoolManagement.Service.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagement.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IConfiguration _configuration;

    public AuthController(IJwtTokenGenerator tokenGenerator, IConfiguration configuration)
    {
        _tokenGenerator = tokenGenerator;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Autenticar e gerar token JWT",
        Description = "Valida as credenciais do administrador e retorna um token JWT com informações de acesso.")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        var adminSection = _configuration.GetSection("AdminCredentials");
        var adminEmail = adminSection.GetValue<string>("Email") ?? string.Empty;
        var adminPasswordHash = adminSection.GetValue<string>("PasswordHash");
        var adminPasswordPlain = adminSection.GetValue<string>("Password");

        if (!string.Equals(request.Email, adminEmail, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized();
        }

        var passwordMatches = false;
        if (!string.IsNullOrEmpty(adminPasswordHash))
        {
            passwordMatches = PasswordHasher.Verify(request.Password, adminPasswordHash);
        }
        else if (!string.IsNullOrEmpty(adminPasswordPlain))
        {
            passwordMatches = adminPasswordPlain == request.Password;
        }

        if (!passwordMatches)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, adminEmail),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Sub, adminEmail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var (token, expiresAt) = _tokenGenerator.GenerateToken(claims);

        return Ok(new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt
        });
    }
}