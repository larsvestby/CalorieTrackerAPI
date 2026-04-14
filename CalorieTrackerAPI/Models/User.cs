using CalorieTrackerAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalorieTrackerAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        [Range(0, 500)]
        public double Weight { get; set; }

        [Range(0, 300)]
        public double Height { get; set; }

        [Required]
        public Gender SelectedGender { get; set; }

        public int ActivityLevelID { get; set; }
        public int GoalID { get; set; }

        [Required]
        public required ActivityLevel ActivityLevel { get; set; }
        [Required]
        public required Goal Goal { get; set; }

        public ICollection<Meal>? Meals { get; set; }
        public ICollection<WaterLog>? WaterLogs { get; set; }
    }
}