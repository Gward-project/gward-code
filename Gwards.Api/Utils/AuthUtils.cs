using Gwards.Api.Models.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Gwards.Api.Utils;

public class AuthUtils
{
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthUtils(JwtConfiguration options)
    {
        _jwtConfiguration = options;
    }

    public string GenerateAccessToken(long userId)
    {
        return CreateToken(_jwtConfiguration.AccessTokenLifetimeMin, userId);
    }

    public Guid GetUserId(string token)
    {
        var authOptions = new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _jwtConfiguration.Issuer,
            ValidAudience = _jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, authOptions, out var securityToken);

        var jwtToken = securityToken as JwtSecurityToken;

        var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId");

        return Guid.Parse(userIdClaim.Value);
    }

    public string GeneratePasswordHash(string password, string salt)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string passwordHash, string salt)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(salt));

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(passwordHash));
    }

    private string CreateToken(int lifetimeInMin, long userId)
    {
        var token = new JwtSecurityToken(
           issuer: _jwtConfiguration.Issuer,
           audience: _jwtConfiguration.Audience,
           claims: new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString(CultureInfo.InvariantCulture)) },
           expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(lifetimeInMin)),
           signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Secret)), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
