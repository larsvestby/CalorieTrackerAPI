using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTrackerAPI.Data;
using CalorieTrackerAPI.Models.DTOs;
using CalorieTrackerAPI.Services.Interfaces;
using System.Security.Claims;

namespace CalorieTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NutritionController(AppDbContext context, INutritionService nutritionService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly INutritionService _nutritionService = nutritionService;

    private int GetCurrentUserId() =>
        int.Parse(User.Claims.First(c => c.Type == "userId").Value);

    // GET api/nutrition/me
    [HttpGet("me")]
    public async Task<IActionResult> GetMyNutrition()
    {
        var userId = GetCurrentUserId();

        var user = await _context.Users
            .Include(u => u.ActivityLevel)
            .Include(u => u.Goal)
            .FirstOrDefaultAsync(u => u.UserID == userId);

        if (user == null)
            return NotFound("User not found");

        var bmr = _nutritionService.CalculateBMR(user.Age, user.Height, user.Weight, user.SelectedGender);
        var tdee = _nutritionService.CalculateTDEE(bmr, user.ActivityLevel.Multiplier);
        var dailyCalories = _nutritionService.GetDailyCalorieTarget(tdee, user.Goal.CalorieAdjustment);
        var macros = _nutritionService.CalculateMacros(user.Weight, dailyCalories);

        return Ok(new NutritionResponseDto
        {
            BMR = Math.Round(bmr),
            TDEE = Math.Round(tdee),
            DailyCalories = dailyCalories,
            Macros = macros
        });
    }
}