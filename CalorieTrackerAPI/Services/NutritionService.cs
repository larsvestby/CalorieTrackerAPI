using CalorieTrackerAPI.Models.DTOs;
using CalorieTrackerAPI.Services.Interfaces;
using System;

namespace CalorieTrackerAPI.Services;

public class NutritionService : INutritionService
{
    // Mifflin-St Jeor
    public double CalculateBMR(int age, double heightCm, double weightKg, Models.Enums.Gender gender)
    {
        double bmr = 10 * weightKg + 6.25 * heightCm - 5 * age;
        bmr += (gender == Models.Enums.Gender.Male) ? 5 : -161;
        return bmr;
    }

    public double CalculateTDEE(double bmr, double activityMultiplier)
    {
        return bmr * activityMultiplier;
    }

    public int GetDailyCalorieTarget(double tdee, int calorieAdjustment)
    {
        return (int)Math.Round(tdee + calorieAdjustment);
    }

    public MacroTargetDto CalculateMacros(double weightKg, int dailyCalories)
    {
        double proteinGrams = weightKg * 1.6;
        double proteinCalories = proteinGrams * 4;

        double fatCalories = dailyCalories * 0.25;
        double fatGrams = fatCalories / 9;

        double carbsCalories = dailyCalories - proteinCalories - fatCalories;
        double carbsGrams = carbsCalories / 4;

        return new MacroTargetDto
        {
            ProteinGrams = Math.Round(proteinGrams, 1),
            CarbsGrams = Math.Round(carbsGrams, 1),
            FatGrams = Math.Round(fatGrams, 1)
        };
    }
}