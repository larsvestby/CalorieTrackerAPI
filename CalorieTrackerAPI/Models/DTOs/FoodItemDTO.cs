using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class FoodItemDTO
    {
    }

    public class CreateFoodItemDto
    {
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
    }

    public class UpdateFoodItemDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        [Range(0, 2000)]
        public double? Calories { get; set; }

        [Range(0, 100)]
        public double? Protein { get; set; }

        [Range(0, 100)]
        public double? Carbohydrates { get; set; }

        [Range(0, 100)]
        public double? Fat { get; set; }
    }

    public class FoodItemResponseDto
    {
        public int FoodItemID { get; set; }
        public required string Name { get; set; }
        public string? Brand { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fat { get; set; }
    }
}
