namespace CalorieTrackerAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FoodItem
    {
        [Key]
        public int FoodItemID { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        [Required]
        [Range(0, 2000)]
        public double Calories { get; set; }

        [Required]
        [Range(0, 100)]
        public double Protein { get; set; }

        [Required]
        [Range(0, 100)]
        public double Carbohydrates { get; set; }

        [Required]
        [Range(0, 100)]
        public double Fat { get; set; }

        public ICollection<MealEntry>? MealEntries { get; set; }
    }
}
