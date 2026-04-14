namespace CalorieTrackerAPI.Models
{
    using CalorieTrackerAPI.Models.Enums;
    using System.ComponentModel.DataAnnotations;

    public class Meal
    {
        [Key]
        public int MealID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public MealType SelectedMealType { get; set; }

        public required User User { get; set; }
        public ICollection<MealEntry>? MealEntries { get; set; }
    }
}
