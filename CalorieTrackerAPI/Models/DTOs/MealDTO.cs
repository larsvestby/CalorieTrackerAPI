using CalorieTrackerAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class MealDTO
    {
    }

    public class CreateMealDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public MealType SelectedMealType { get; set; }
    }

    public class UpdateMealDto
    {
        public DateTime? Date { get; set; }
        public MealType? SelectedMealType { get; set; }
    }

    public class MealResponseDto
    {
        public int MealID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public MealType SelectedMealType { get; set; }
        public List<MealEntryResponseDto> MealEntries { get; set; } = [];

        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalCarbohydrates { get; set; }
        public double TotalFat { get; set; }
    }
}
