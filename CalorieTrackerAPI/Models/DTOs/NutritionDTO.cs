using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs;

public class NutritionRequestDto
{
    [Range(1, 120)]
    public int Age { get; set; }

    [Range(1, 300)]
    public double Height { get; set; }

    [Range(1, 500)]
    public double Weight { get; set; }

    [Required]
    public Enums.Gender Gender { get; set; }

    [Required]
    public int ActivityLevelID { get; set; }

    [Required]
    public int GoalID { get; set; }
}

public class NutritionResponseDto
{
    public double BMR { get; set; }
    public double TDEE { get; set; }
    public int DailyCalories { get; set; }
    public MacroTargetDto Macros { get; set; } = new();
}

public class MacroTargetDto
{
    public double ProteinGrams { get; set; }
    public double CarbsGrams { get; set; }
    public double FatGrams { get; set; }
}