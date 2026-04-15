using CalorieTrackerAPI.Models;

namespace CalorieTrackerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
