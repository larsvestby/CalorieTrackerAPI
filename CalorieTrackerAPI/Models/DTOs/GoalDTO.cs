using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class GoalDTO
    {
    }

    public class CreateGoalDto
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [Range(-1000, 1000)]
        public int CalorieAdjustment { get; set; }
    }

    public class UpdateGoalDto
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [Range(-1000, 1000)]
        public int? CalorieAdjustment { get; set; }
    }

    public class GoalResponseDto
    {
        public int GoalID { get; set; }
        public required string Name { get; set; }
        public int CalorieAdjustment { get; set; }
    }
}
