using CalorieTrackerAPI.Models.DTOs;
using CalorieTrackerAPI.Models.Enums;
using CalorieTrackerAPI.Services;

namespace CalorieTrackerTests;

public class NutritionServiceTests
{
    private readonly NutritionService _nutritionService;

    public NutritionServiceTests()
    {
        _nutritionService = new NutritionService();
    }

    [Theory]
    [InlineData(30, 180, 80, Gender.Male, 1780)]
    [InlineData(30, 165, 60, Gender.Female, 1320.25)]
    [InlineData(1, 1, 1, Gender.Male, 16.25)]
    public void CalculateBMR_ReturnsCorrectValue(int age, double heightCm, double weightKg, Gender gender, double expected)
    {
        double result = _nutritionService.CalculateBMR(age, heightCm, weightKg, gender);

        Assert.Equal(expected, result, 2);
    }

    [Theory]
    [InlineData(1800, ActivityLevels.Moderate, 2790)]
    [InlineData(1500, ActivityLevels.Sedentary, 1800)]
    [InlineData(2000, ActivityLevels.Extreme, 3800)]
    public void CalculateTDEE_ReturnsCorrectValue(double bmr, double activityMultiplier, double expected)
    {
        double result = _nutritionService.CalculateTDEE(bmr, activityMultiplier);

        Assert.Equal(expected, result, 2);
    }

    [Theory]
    [InlineData(2500, Goals.WeightLoss, 2000)]
    [InlineData(1999.9, Goals.Maintain, 2000)]
    [InlineData(2200.4, Goals.MuscleGain, 2500)]
    public void GetDailyCalorieTarget_ReturnsCorrectValue(double tdee, int calorieAdjustment, int expected)
    {
        int result = _nutritionService.GetDailyCalorieTarget(tdee, calorieAdjustment);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(70, 2000, 112.0, 263.0, 55.6)]
    [InlineData(100, 2500, 160.0, 308.8, 69.4)]
    public void CalculateMacros_ReturnsCorrectValues(double weightKg, int dailyCalories, double expectedProtein, double expectedCarbs, double expectedFat)
    {
        MacroTargetDto result = _nutritionService.CalculateMacros(weightKg, dailyCalories);

        Assert.Equal(expectedProtein, result.ProteinGrams, 1);
        Assert.Equal(expectedCarbs, result.CarbsGrams, 1);
        Assert.Equal(expectedFat, result.FatGrams, 1);
    }

    [Theory]
    [InlineData(30, 180, 80, Gender.Male, ActivityLevels.Moderate, Goals.WeightLoss, 2259, 128.0, 295.6, 62.8)]
    [InlineData(30, 180, 80, Gender.Male, ActivityLevels.Moderate, Goals.Maintain, 2759, 128.0, 389.3, 76.6)]
    [InlineData(25, 175, 75, Gender.Male, ActivityLevels.Extreme, Goals.MuscleGain, 3575, 120.0, 550.3, 99.3)]
    public void CalculateNutritionFlow_ReturnsCorrectFinalValues(
    int age,
    double heightCm,
    double weightKg,
    Gender gender,
    double activityMultiplier,
    int calorieAdjustment,
    int expectedDailyCalories,
    double expectedProtein,
    double expectedCarbs,
    double expectedFat)
    {
        double bmr = _nutritionService.CalculateBMR(age, heightCm, weightKg, gender);
        double tdee = _nutritionService.CalculateTDEE(bmr, activityMultiplier);
        int dailyCalories = _nutritionService.GetDailyCalorieTarget(tdee, calorieAdjustment);
        var macros = _nutritionService.CalculateMacros(weightKg, dailyCalories);

        Assert.Equal(expectedDailyCalories, dailyCalories);
        Assert.Equal(expectedProtein, macros.ProteinGrams, 1);
        Assert.Equal(expectedCarbs, macros.CarbsGrams, 1);
        Assert.Equal(expectedFat, macros.FatGrams, 1);
    }
}

public static class ActivityLevels
{
    public const double Sedentary = 1.2;
    public const double Light = 1.375;
    public const double Moderate = 1.55;
    public const double High = 1.725;
    public const double Extreme = 1.9;
}

public static class Goals
{
    public const int WeightLoss = -500;
    public const int Maintain = 0;
    public const int MuscleGain = 300;
}