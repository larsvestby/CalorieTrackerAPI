namespace CalorieTrackerAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WaterLog
    {
        [Key]
        public int WaterLogID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 10000)]
        public int MlIngested { get; set; }

        public required User User { get; set; }
    }
}
