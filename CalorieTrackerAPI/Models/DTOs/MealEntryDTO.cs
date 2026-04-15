using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class MealEntryDTO
    {
    }

    public class CreateMealEntryDto
    {
        [Required]
        public int MealID { get; set; }

        [Required]
        public int FoodItemID { get; set; }

        [Required]
        [Range(1, 5000)]
        public double QuantityInGrams { get; set; }
    }

    public class UpdateMealEntryDto
    {
        [Range(1, 5000)]
        public double? QuantityInGrams { get; set; }
    }

    public class MealEntryResponseDto
    {
        public int MealEntryID { get; set; }
        public int MealID { get; set; }
        public required FoodItemResponseDto FoodItem { get; set; }
        public double QuantityInGrams { get; set; }
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalCarbohydrates { get; set; }
        public double TotalFat { get; set; }
    }
}
