using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public bool ValidateToken(string token)
    {
        var secretKey = _configuration["Authentication:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");
        }
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Authentication:Issuer"],
                ValidAudience = _configuration["Authentication:Audience"],
                IssuerSigningKey = securityKey
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }


    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthRequestBody authRequestBody)
    {
        if (IsValidUser(authRequestBody))
        {
            var tokenString = GenerateJwtToken(authRequestBody.Username);
            if (tokenString == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Key Configuration Error.");
            }

            return Ok(new { Token = tokenString });
        }

        return Unauthorized();
    }

    private bool IsValidUser(AuthRequestBody authRequestBody)
    {
        return authRequestBody.Username == "admin" && authRequestBody.Password == "admin";
    }

    private string? GenerateJwtToken(string username)
    {
        var secretKey = _configuration["Authentication:SecretKey"]?.Trim();
        if (string.IsNullOrEmpty(secretKey))
        {
            Console.WriteLine("Secret key is null or empty!");
            return null;
        }

        try
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            string jwtToken = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated JWT Token: {jwtToken}");

            return jwtToken;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error generating JWT: {ex}");
            return null;
        }
    }
}
