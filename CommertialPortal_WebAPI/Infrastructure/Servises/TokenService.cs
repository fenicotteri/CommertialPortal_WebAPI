namespace CommertialPortal_WebAPI.Infrastructure.Servises;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CommertialPortal_WebAPI.API;
using CommertialPortal_WebAPI.Application.Interfaces;
using CommertialPortal_WebAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var roleClaim = user.UserType switch
        {
            UserType.Business => new Claim(ClaimTypes.Role, AuthRoles.Business),
            UserType.Client => new Claim(ClaimTypes.Role, AuthRoles.Client),
            _ => throw new InvalidOperationException("Unknown user type.")
        };

        claims.Add(roleClaim);

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}