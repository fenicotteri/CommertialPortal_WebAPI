using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommertialPortal_WebAPI.Domain.Entities;
using CommertialPortal_WebAPI.Infrastructure.Servises;
using Microsoft.Extensions.Configuration;

namespace TestCommertialPortal;

public class TokenServiceTests
{
    private TokenService CreateTokenService()
    {
        var inMemorySettings = new Dictionary<string, string> {
                {"JWT:SigningKey", "THIS_IS_A_TEST_SIGNING_KEY_FOR_UNIT_TESTING_PURPOSES_64_CHARACTERS_LONG____"},
                {"JWT:Issuer", "TestIssuer"},
                {"JWT:Audience", "TestAudience"}
            };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        return new TokenService(configuration);
    }

    [Fact]
    public void CreateToken_ShouldGenerateToken_ForBusinessUser()
    {
        // Arrange
        var tokenService = CreateTokenService();
        var user = new User
        {
            Email = "business@example.com",
            UserType = UserType.Business
        };

        // Act
        var token = tokenService.CreateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Contains(jwt.Claims, c => c.Type == "email" && c.Value == user.Email);
        Assert.Contains(jwt.Claims, c => c.Type == "role" && c.Value == "Business");
    }

    [Fact]
    public void CreateToken_ShouldGenerateToken_ForClientUser()
    {
        // Arrange
        var tokenService = CreateTokenService();
        var user = new User
        {
            Email = "client@example.com",
            UserType = UserType.Client
        };

        // Act
        var token = tokenService.CreateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Contains(jwt.Claims, c => c.Type == "email" && c.Value == user.Email);
        Assert.Contains(jwt.Claims, c => c.Type == "role" && c.Value == "Client");
    }

    [Fact]
    public void CreateToken_ShouldThrowException_WhenUserTypeIsUnknown()
    {
        // Arrange
        var tokenService = CreateTokenService();
        var user = new User
        {
            Email = "unknown@example.com",
            UserType = null // Ломаем кейс
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tokenService.CreateToken(user));
    }
}
