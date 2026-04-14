namespace CalorieTrackerAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ActivityLevel
    {
        [Key]
        public int ActivityLevelID { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public double Multiplier { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
