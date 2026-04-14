namespace CalorieTrackerAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Goal
    {
        [Key]
        public int GoalID { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public int CalorieAdjustment { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
