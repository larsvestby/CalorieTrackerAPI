using System.ComponentModel.DataAnnotations;

namespace CalorieTrackerAPI.Models.DTOs
{
    public class ActivityLevelDTO
    {
    }

    public class CreateActivityLevelDto
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [Range(0.1, 10.0)]
        public double Multiplier { get; set; }
    }

    public class UpdateActivityLevelDto
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [Range(0.1, 10.0)]
        public double? Multiplier { get; set; }
    }

    public class ActivityLevelResponseDto
    {
        public int ActivityLevelID { get; set; }
        public required string Name { get; set; }
        public double Multiplier { get; set; }
    }
}
