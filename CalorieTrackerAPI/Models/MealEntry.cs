namespace CalorieTrackerAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MealEntry
    {
        [Key]
        public int MealEntryID { get; set; }

        [Required]
        public int MealID { get; set; }

        [Required]
        public int FoodItemID { get; set; }

        [Required]
        [Range(1, 5000)]
        public double QuantityInGrams { get; set; }

        public required Meal Meal { get; set; }
        public required FoodItem FoodItem { get; set; }
    }
}
