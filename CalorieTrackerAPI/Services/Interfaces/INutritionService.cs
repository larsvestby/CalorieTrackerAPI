using CalorieTrackerAPI.Models.DTOs;
using System;

namespace CalorieTrackerAPI.Services.Interfaces;

public interface INutritionService
{
    double CalculateBMR(int age, double heightCm, double weightKg, Models.Enums.Gender gender);
    double CalculateTDEE(double bmr, double activityMultiplier);
    int GetDailyCalorieTarget(double tdee, int calorieAdjustment);
    MacroTargetDto CalculateMacros(double weightKg, int dailyCalories);
}