using CalorieTrackerAPI.Models;
using CalorieTrackerAPI.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CalorieTrackerTests;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:Key", "ThisIsASuperLongSecretKeyAtLeast32Chars!"},
            {"Jwt:Issuer", "CalorieTrackerAPI"},
            {"Jwt:Audience", "CalorieTrackerUsers"},
            {"Jwt:ExpiresInMinutes", "60"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _authService = new AuthService(configuration);
    }

    [Fact]
    public void HashPassword_ReturnsDifferentHashes()
    {
        string password = "SecretPassword";

        string hash1 = _authService.HashPassword(password);
        string hash2 = _authService.HashPassword(password);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void VerifyPassword_CorrectPassword()
    {
        string password = "CorrectPassword";
        string hash = _authService.HashPassword(password);

        bool result = _authService.VerifyPassword(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WrongPassword()
    {
        string password = "CorrectPassword";
        string hash = _authService.HashPassword(password);

        bool result = _authService.VerifyPassword("WrongPassword", hash);

        Assert.False(result);
    }

    [Fact]
    public void GenerateJwtToken_ReturnsToken()
    {
        var user = CreateTestUser();

        var token = _authService.GenerateJwtToken(user);

        Assert.NotNull(token);
    }

    [Fact]
    public void GenerateJwtToken_ContainsCorrectClaims()
    {
        var user = CreateTestUser();

        var token = _authService.GenerateJwtToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal(user.Email, jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
        Assert.Equal(user.UserID.ToString(), jwt.Claims.First(c => c.Type == "userId").Value);
    }

    [Fact]
    public void GenerateJwtToken_IsNotExpired()
    {
        var user = CreateTestUser();
        var token = _authService.GenerateJwtToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.True(jwt.ValidTo > DateTime.UtcNow);
    }

    private static User CreateTestUser()
    {
        return new User
        {
            UserID = 42,
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            PasswordHash = "SecretPassword",
            Age = 26,
            Weight = 100,
            Height = 182,
            SelectedGender = CalorieTrackerAPI.Models.Enums.Gender.Male,
            ActivityLevel = new ActivityLevel
            {
                ActivityLevelID = 1,
                Name = "Sedentary",
                Multiplier = 1.2
            },
            Goal = new Goal
            {
                GoalID = 1,
                Name = "Weightloss",
                CalorieAdjustment = -500
            }
        };
    }
}